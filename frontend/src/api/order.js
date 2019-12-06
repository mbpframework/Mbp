import request from '@/utils/request'

export function fetchList(query) {
  return request({
    url: '/api/services/app/Order/GetOrderListAsync',
    method: 'get',
    params: query
  })
}

export function fetchArticle(orderInfoId) {
  return request({
    url: '/api/services/app/Order/GetOrderDetailAsync',
    method: 'get',
    params: { orderInfoId }
  })
}

export function fetchPv(pv) {
  return request({
    url: '/article/pv',
    method: 'get',
    params: { pv }
  })
}

export function createArticle(data) {
  return request({
    url: '/article/create',
    method: 'post',
    data
  })
}

export function updateArticle(data) {
  return request({
    url: '/article/update',
    method: 'post',
    data
  })
}
