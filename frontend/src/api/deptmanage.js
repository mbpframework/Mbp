import request from '@/utils/request'

// 添加部门
export function AddDept(data) {
  return request({
    url: '/DeptManage/AddDept',
    method: 'post',
    data
  })
}

// 更新部门
export function UpdateDept(data) {
  return request({
    url: '/DeptManage/UpdateDept',
    method: 'put',
    data
  })
}

// 获取部门列表
export function GetDepts(data) {
  return request({
    url: '/DeptManage/GetDepts',
    method: 'get',
    params: { 'pageSize': data.pageSize, 'pageIndex': data.pageIndex,
      'Search.Name': data.Name, 'Search.Code': data.Code, 'Search.SystemCode': data.SystemCode }
  })
}

// 删除部门,单条
export function DeleteDept(deptId) {
  return request({
    url: '/DeptManage/DeleteDept',
    method: 'delete',
    params: { deptId }
  })
}

// 删除部门,单条
export function DeleteDepts(deptIds) {
  return request({
    url: '/DeptManage/DeleteDepts',
    method: 'delete',
    data: deptIds
  })
}

