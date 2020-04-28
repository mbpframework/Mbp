import request from '@/utils/request'

// 获取附件
export function GetFiles(bussinessId) {
  return request({
    url: '/Attachment/GetAttachments',
    method: 'get',
    params: { 'bussinessId': bussinessId }
  })
}

// 上传附件
export function UpLoadFile(data) {
  return request({
    url: '/Attachment/UpLoadFile',
    method: 'post',
    data
  })
}

// 清理临时目录
export function ClearUserTemp(data) {
  return request({
    url: '/Attachment/ClearUserTemp',
    method: 'put',
    data
  })
}

// 下载附件
export function FetchAttachment(name, url) {
  return request({
    url: '/Attachment/FetchAttachment',
    method: 'get',
    params: { 'fileName': name, 'url': url }
  })
}

