import request from '@/utils/request'

// 添加功能菜单
export function AddMenu(data) {
  return request({
    url: '/MenuManage/AddMenu',
    method: 'post',
    data
  })
}

// 更新菜单
export function UpdateMenu(data) {
  return request({
    url: '/MenuManage/UpdateMenu',
    method: 'put',
    data
  })
}

// 获取菜单列表
export function GetMenus(data) {
  return request({
    url: '/MenuManage/GetMenus',
    method: 'get',
    params: { 'pageSize': data.pageSize, 'pageIndex': data.pageIndex,
      'Search.Name': data.Name, 'Search.Code': data.Code, 'Search.SystemCode': data.SystemCode }
  })
}

// 配置功能操作权限
export function AddMenuClaims(menuId, claims) {
  return request({
    url: '/MenuManage/AddMenuClaims',
    method: 'post',
    data: claims
  })
}

// 删除菜单,单条
export function DeleteMenu(menuId) {
  return request({
    url: '/MenuManage/DeleteMenu',
    method: 'delete',
    params: { menuId }
  })
}

// 删除菜单,单条
export function DeleteMenus(menuIds) {
  return request({
    url: '/MenuManage/DeleteMenus',
    method: 'delete',
    data: menuIds
  })
}

