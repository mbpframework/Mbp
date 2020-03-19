import request from '@/utils/request'

export function login(data) {
  return request({
    url: '/Account/Login',
    method: 'get',
    params: { LoginName: data.LoginName, Password: data.Password, ClientID: data.ClientID }
  })
}

// 暂时获取信息
export function getInfo() {
  return request({
    url: '/Account/GetUserInfo',
    method: 'get'
  })
}

export function logout() {
  return request({
    url: '/Account/LogOut',
    method: 'get'
  })
}
