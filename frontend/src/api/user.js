import request from '@/utils/request'

export function login(data) {
  return request({
    url: '/Account/Login',
    method: 'get',
    params: { LoginName: data.LoginName, Password: data.Password, ClientID: data.ClientID }
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
    url: '/Account/LogOut',
    method: 'get'
  })
}
