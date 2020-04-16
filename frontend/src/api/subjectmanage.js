import request from '@/utils/request'

// 获取科目信息
export function GetSubjects(data) {
  return request({
    url: '/TrainSubject/GetSubjects',
    method: 'get',
    params: { 'pageSize': data.pageSize, 'pageIndex': data.pageIndex, 'Search.Name': data.SubjectName, 'Search.Code': data.SubjectCode,
      'Search.TrainType': data.TrainType, 'Search.SystemCode': data.SystemCode }
  })
}

// 添加科目
export function AddSubject(data) {
  return request({
    url: '/TrainSubject/AddSubject',
    method: 'post',
    data
  })
}

// 删除科目
export function DeleteSubject(SubjectId) {
  return request({
    url: '/TrainSubject/DeleteSubject',
    method: 'delete',
    params: { SubjectId }
  })
}

// 更新科目
export function UpdateSubject(data) {
  return request({
    url: '/TrainSubject/UpdateSubject',
    method: 'put',
    data
  })
}

