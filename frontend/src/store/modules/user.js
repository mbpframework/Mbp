import { login, logout, getInfo } from '@/api/user'
import { getToken, setToken, removeToken, getRefreshToken, setRefreshToken, removeRefreshToken } from '@/utils/auth'
import { resetRouter } from '@/router'
import { guid } from '@/utils/uuid'

const state = {
  token: getToken(),
  refreshToken: getRefreshToken(),
  name: '',
  avatar: '',
  roles: []
}

const mutations = {
  SET_TOKEN: (state, token) => {
    state.token = token
  },
  SET_RRTOKEN: (state, refreshToken) => {
    state.refreshToken = refreshToken
  },
  SET_NAME: (state, name) => {
    state.name = name
  },
  SET_AVATAR: (state, avatar) => {
    state.avatar = avatar
  },
  SET_ROLES: (state, roles) => {
    state.roles = roles
  }
}

const actions = {
  // 用户登录
  login({ commit }, userInfo) {
    const { username, password } = userInfo
    return new Promise((resolve, reject) => {
      login({ LoginName: username.trim(), Password: password, ClientID: guid() })
        .then(response => {
          // 保存登录token到vuex
          commit('SET_TOKEN', response.Data.AccessToken.AccessToken)
          // 保存token到cookies
          setToken(response.Data.AccessToken.AccessToken)
          // 保存刷新token到vuex
          commit('SET_RRTOKEN', response.Data.AccessToken.RefreshToken)
          // 保存刷新token到cookies
          setRefreshToken(response.Data.AccessToken.RefreshToken)

          const { Role, UserName } = response.Data

          commit('SET_NAME', UserName)
          commit('SET_AVATAR', 'avatar')
          commit('SET_ROLES', Role)
          resolve()
        }).catch(error => {
          reject(error)
        })
    })
  },

  // 获取用户信息
  getInfo({ commit, state }) {
    return new Promise((resolve, reject) => {
      getInfo(state.token).then(response => {
        const data = response

        if (!data.Success) {
          reject('Verification failed, please Login again.')
        }

        // 先忽略权限
        data.roles = ['admin']
        const { roles, name, avatar } = data

        // roles must be a non-empty array
        if (!roles || roles.length <= 0) {
          reject('getInfo: roles must be a non-null array!')
        }

        commit('SET_NAME', name)
        commit('SET_AVATAR', avatar)
        commit('SET_ROLES', roles)
        resolve(data)
      }).catch(error => {
        reject(error)
      })
    })
  },

  // 用户注销
  logout({ commit, state }) {
    return new Promise((resolve, reject) => {
      logout(state.token).then(() => {
        // token和刷新token的清空
        commit('SET_TOKEN', '')
        commit('SET_RRTOKEN', '')
        removeToken()
        removeRefreshToken()
        resetRouter()
        resolve()
      }).catch(error => {
        reject(error)
      })
    })
  },

  // remove token
  resetToken({ commit }) {
    return new Promise(resolve => {
      commit('SET_TOKEN', '')
      commit('SET_RRTOKEN', '')
      removeToken()
      removeRefreshToken()
      resolve()
    })
  }
}

export default {
  namespaced: true,
  state,
  mutations,
  actions
}

