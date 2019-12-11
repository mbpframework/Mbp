import request from '@/utils/request'

// 获取角色信息
export function GetRoles(data) {
  return request({
    url: '/RoleManage/GetRoles',
    method: 'get',
    params: { 'pageSize': data.pageSize, 'pageIndex': data.pageIndex, 'Search.Name': data.RoleName, 'Search.SystemCode': data.SystemCode }
  })
}

// 添加角色
export function AddRole(data) {
  return request({
    url: '/RoleManage/AddRole',
    method: 'post',
    data
  })
}

// 删除角色
export function DeleteRole(data) {
  return request({
    url: '/RoleManage/DeleteRole',
    method: 'delete',
    data
  })
}

// 更新角色
export function UpdateRole(data) {
  return request({
    url: '/RoleManage/UpdateRole',
    method: 'put',
    data
  })
}

// 配置角色功能
export function AddRoleMenus(roleId, menuIds) {
  return request({
    url: '/RoleManage/AddRoleMenus',
    method: 'post',
    params: { roleId },
    data: menuIds
  })
}

// 删除角色功能关系,全部
export function DeleteRoleMenus(roleId) {
  return request({
    url: '/RoleManage/DeleteRoleMenus',
    method: 'delete',
    params: { roleId }
  })
}

// 删除角色功能关系,单条
export function DeleteRoleMenu(roleId, menuId) {
  return request({
    url: '/RoleManage/DeleteRoleMenu',
    method: 'delete',
    params: { roleId, menuId }
  })
}

// 获取角色下的菜单
export function GetRoleMenus(roleId) {
  return request({
    url: '/RoleManage/GetRoleMenus',
    method: 'get',
    params: { roleId }
  })
}
