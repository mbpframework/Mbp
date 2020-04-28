import request from '@/utils/request'

// 获取公告信息
export function GetNotices(data) {
  return request({
    url: '/Notice/GetNotices',
    method: 'get',
    params: { 'pageSize': data.pageSize, 'pageIndex': data.pageIndex, 'Search.Title': data.Title, 'Search.PublishTime': data.PublishTime,
      'Search.NoticeType': data.NoticeType, 'Search.NoticeStatus': data.NoticeStatus }
  })
}

// 添加公告
export function AddNotice(data) {
  return request({
    url: '/Notice/AddNotice',
    method: 'post',
    data
  })
}

// 删除公告
export function DeleteNotice(NoticeId) {
  return request({
    url: '/Notice/DeleteNotice',
    method: 'delete',
    params: { NoticeId }
  })
}

// 更新公告
export function UpdateNotice(data) {
  return request({
    url: '/Notice/UpdateNotice',
    method: 'put',
    data
  })
}

