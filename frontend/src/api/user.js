import request from '@/utils/request'

export function login(data) {
  return request({
    url: '/api/Account/Login',
    method: 'post',
    data
  })
}

export function getInfo(token) {
  return request({
    url: '/api/services/app/Session/GetCurrentLoginInformations',
    method: 'get',
    params: { token }
  })
}

export function logout() {
  return request({
    url: '/user/logout',
    method: 'post'
  })
}
