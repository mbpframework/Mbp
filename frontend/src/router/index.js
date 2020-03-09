import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

/* Layout */
import Layout from '@/layout'

/**
 * Note: sub-menu only appear when route children.length >= 1
 * Detail see: https://panjiachen.github.io/vue-element-admin-site/guide/essentials/router-and-nav.html
 *
 * hidden: true                   if set true, item will not show in the sidebar(default is false)
 * alwaysShow: true               if set true, will always show the root menu
 *                                if not set alwaysShow, when item has more than one children route,
 *                                it will becomes nested mode, otherwise not show the root menu
 * redirect: noRedirect           if set noRedirect will no redirect in the breadcrumb
 * name:'router-name'             the name is used by <keep-alive> (must set!!!)
 * meta : {
    roles: ['admin','editor']    control the page roles (you can set multiple roles)
    title: 'title'               the name show in sidebar and breadcrumb (recommend set)
    icon: 'svg-name'             the icon show in the sidebar
    breadcrumb: false            if set false, the item will hidden in breadcrumb(default is true)
    activeMenu: '/example/list'  if set path, the sidebar will highlight the path you set
  }
 */

/**
 * constantRoutes
 * a base page that does not have permission requirements
 * all roles can be accessed
 */
export const constantRoutes = [{
  path: '/redirect',
  component: Layout,
  hidden: true,
  children: [
    {
      path: '/redirect/:path*',
      component: () => import('@/views/redirect/index')
    }
  ]
},
{
  path: '/login',
  component: () => import('@/views/login/index'),
  hidden: true
},

{
  path: '/404',
  component: () => import('@/views/404'),
  hidden: true
},

{
  path: '/',
  component: Layout,
  redirect: '/dashboard',
  children: [{
    path: 'dashboard',
    name: 'Dashboard',
    component: () => import('@/views/dashboard/index'),
    meta: { title: '首页', icon: 'dashboard', affix: true }
  }]
}, {
  path: '/usermanage',
  component: Layout,
  redirect: '/usermanage/user',
  name: 'user',
  meta: {
    title: '系统管理',
    icon: 'user',
    roles: ['admin']
  },
  children: [
    {
      path: 'dept',
      name: 'DeptManage',
      component: () => import('@/views/usermanage/dept/index'),
      meta: { title: '机构管理' }
    },
    {
      path: 'person',
      name: 'PersonManage',
      component: () => import('@/views/usermanage/person/index'),
      meta: { title: '人员管理' }
    },
    {
      path: 'user',
      name: 'UserManage',
      component: () => import('@/views/usermanage/user/index'),
      meta: { title: '用户管理' }
    },
    {
      path: 'role',
      name: 'RoleManage',
      component: () => import('@/views/usermanage/role/index'),
      meta: { title: '角色管理' }
    },
    {
      path: 'menu',
      name: 'MenuManage',
      component: () => import('@/views/usermanage/menu/index'),
      meta: { title: '菜单管理' }
    },
    {
      path: 'menu',
      name: 'MenuManage',
      component: () => import('@/views/usermanage/menu/index'),
      meta: { title: '岗位管理' }
    },
    {
      path: 'menu',
      name: 'MenuManage',
      component: () => import('@/views/usermanage/menu/index'),
      meta: { title: '科目管理' }
    },
    {
      path: 'menu',
      name: 'MenuManage',
      component: () => import('@/views/usermanage/menu/index'),
      meta: { title: '系统代码管理' }
    },
    {
      path: 'menu',
      name: 'MenuManage',
      component: () => import('@/views/usermanage/menu/index'),
      meta: { title: '文件发布' }
    },
    {
      path: 'menu',
      name: 'MenuManage',
      component: () => import('@/views/usermanage/menu/index'),
      meta: { title: '流程管理' }
    },
    {
      path: 'menu',
      name: 'MenuManage',
      component: () => import('@/views/usermanage/menu/index'),
      meta: { title: '权限管理' }
    }
  ]
},
{
  path: '/bussiness',
  component: Layout,
  name: 'bussiness',
  meta: {
    title: '业务功能',
    icon: 'excel',
    roles: ['admin']
  },
  children: [{
    path: 'plan',
    name: 'PlanManage',
    component: () => import('@/views/bussiness/plan/index'),
    meta: { title: '训练计划录入' }
  },
  {
    path: 'report',
    name: 'PlanManage',
    component: () => import('@/views/bussiness/report/index'),
    meta: { title: '训练报告录入' }
  },
  {
    path: 'meeting',
    name: 'PlanManage',
    component: () => import('@/views/bussiness/meeting/index'),
    meta: { title: '训练会议录入' }
  },
  {
    path: 'score',
    name: 'PlanManage',
    component: () => import('@/views/bussiness/score/index'),
    meta: { title: '训练成绩录入' }
  },
  {
    path: 'statistical',
    name: 'PlanManage',
    component: () => import('@/views/bussiness/statistical/index'),
    meta: { title: '训练统计报表' }
  },
  {
    path: 'datafx',
    name: 'PlanManage',
    component: () => import('@/views/bussiness/datafx/index'),
    meta: { title: '统计数据分析' }
  },
  {
    path: 'rules',
    name: 'PlanManage',
    component: () => import('@/views/bussiness/rules/index'),
    meta: { title: '文件查询浏览' }
  }]
},
{
  path: '/logmanage',
  component: Layout,
  children: [{
    path: 'loglist',
    name: 'loglist',
    component: () => import('@/views/log/index'),
    meta: { title: '日志列表', icon: 'documentation' }
  }]
}
]

export const asyncRoutes = [
  // 404 page must be placed at the end !!!
  { path: '*', redirect: '/404', hidden: true }]

const createRouter = () => new Router({
  // mode: 'history', // require service support
  scrollBehavior: () => ({ y: 0 }),
  routes: constantRoutes
})

const router = createRouter()

// Detail see: https://github.com/vuejs/vue-router/issues/1234#issuecomment-357941465
export function resetRouter() {
  const newRouter = createRouter()
  router.matcher = newRouter.matcher // reset router
}

export default router
