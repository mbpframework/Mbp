import request from '@/utils/request'

// 添加部门
export function AddPosition(data) {
  return request({
    url: '/PositionManage/AddPosition',
    method: 'post',
    data
  })
}

// 更新部门
export function UpdatePosition(data) {
  return request({
    url: '/PositionManage/UpdatePosition',
    method: 'put',
    data
  })
}

// 获取部门列表
export function GetPositions(data) {
  return request({
    url: '/PositionManage/GetPositions',
    method: 'get',
    params: { 'pageSize': data.pageSize, 'pageIndex': data.pageIndex,
      'Search.Name': data.Name, 'Search.Code': data.Code, 'Search.SystemCode': data.SystemCode }
  })
}

// 删除部门,单条
export function DeletePosition(PositionId) {
  return request({
    url: '/PositionManage/DeletePosition',
    method: 'delete',
    params: { PositionId }
  })
}

// 删除部门,单条
export function DeletePositions(PositionIds) {
  return request({
    url: '/PositionManage/DeletePositions',
    method: 'delete',
    data: PositionIds
  })
}

