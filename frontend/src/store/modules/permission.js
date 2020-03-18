import { constantRoutes } from '@/router'
import Layout from '@/layout'
import { GetMenusForRoute } from '@/api/menumanage'

/**
 * Use meta.role to determine if the current user has permission
 * @param roles
 * @param route
 */
function hasPermission(roles, route) {
  if (route.meta && route.meta.roles) {
    return roles.some(role => route.meta.roles.includes(role))
  } else {
    return true
  }
}

/**
 * Filter asynchronous routing tables by recursion
 * @param routes asyncRoutes
 * @param roles
 */
export function filterAsyncRoutes(routes, roles) {
  const res = []

  routes.forEach(route => {
    const tmp = { ...route }
    if (hasPermission(roles, tmp)) {
      if (tmp.children) {
        tmp.children = filterAsyncRoutes(tmp.children, roles)
      }
      res.push(tmp)
    }
  })

  return res
}

const state = {
  routes: [],
  addRoutes: []
}

const mutations = {
  SET_ROUTES: (state, routes) => {
    state.addRoutes = routes
    state.routes = constantRoutes.concat(routes)
  }
}
// const path = 'log'
// const remoteRoutes = [{
//   path: '/logmanage',
//   component: Layout,
//   children: [{
//     path: 'loglist',
//     name: 'loglist',
//     component: () => import(`@/views/${path}/index`),
//     meta: { title: '日志列表', icon: 'documentation' }
//   }]
// }]

const actions = {
  generateRoutes({ commit }, roles) {
    return new Promise(resolve => {
      // 申明可访问的路由
      // let accessedRoutes
      // if (roles.includes('admin')) {
      //   // 如果是管理员所有动态路由都可以访问
      //   accessedRoutes = asyncRoutes || []
      // } else {
      //   // 根据权限过滤动态路由
      //   accessedRoutes = filterAsyncRoutes(asyncRoutes, roles)
      // }
      GetMenusForRoute().then(response => {
        const remoteRoutes = generateRoutes(response.Data)
        commit('SET_ROUTES', remoteRoutes)
        resolve(remoteRoutes)
      })
    })
  }
}

function generateRoutes(menus) {
  const routes = menus.map(menu => {
    if (menu.component === 'Layout') {
      menu.component = Layout
    } else {
      const name = menu.component
      menu.component = () => import(`@/views${name}/index`)
    }

    if (menu.children && menu.children.length) {
      menu.children = generateRoutes(menu.children)
    }
    return menu
  })
  return routes
}

export default {
  namespaced: true,
  state,
  mutations,
  actions
}
