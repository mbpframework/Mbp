import request from '@/utils/request'

// 添加用户
export function AddUser(data) {
  return request({
    url: '/UserManage/AddUse',
    method: 'post',
    data
  })
}

// 更新用户
export function UpdateUser(data) {
  return request({
    url: '/UserManage/UpdateUser',
    method: 'put',
    data
  })
}

// 获取用户
export function GetUser(data) {
  return request({
    url: '/UserManage/GetUsers',
    method: 'get',
    params: { pageSize: data.pageSize, pageIndex: data.pageIndex }
  })
}

export function DeleteUser(userId) {
  return request({
    url: '/UserManage/DeleteUser',
    method: 'delete',
    params: { userId }
  })
}

// 添加用户角色
export function AddUserRoles(data) {
  return request({
    url: '/UserManage/AddUserRoles',
    method: 'post',
    data
  })
}

// 删除用户角色,多条
export function DeleteUserRoles(data) {
  return request({
    url: '/UserManage/DeleteUserRoles',
    method: 'delete',
    data
  })
}

// 删除用户角色,单条
export function DeleteUserRole(data) {
  return request({
    url: '/UserManage/DeleteUserRole',
    method: 'delete',
    data
  })
}

// 获取用户角色信息
export function GetUserRoles(userId) {
  return request({
    url: '/UserManage/GetUserRoles',
    method: 'get',
    params: { userId }
  })
}
