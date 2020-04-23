import request from '@/utils/request'

// 获取个人训练成绩
export function GetTrainScores(data) {
  return request({
    url: '/TrainScore/GetTrainScores',
    method: 'get',
    params: { 'pageSize': data.pageSize, 'pageIndex': data.pageIndex, 'Search.UserName': data.UserName, 'Search.Major': data.Major,
      'Search.SubjectId': data.SubjectId, 'Search.TrainDate': data.TrainDate, 'Search.Score': data.Score }
  })
}

// 添加个人训练成绩
export function AddTrainScore(data) {
  return request({
    url: '/TrainScore/AddTrainScore',
    method: 'post',
    data
  })
}

// 删除个人训练成绩
export function DeleteTrainScore(trainScoreId) {
  return request({
    url: '/TrainScore/DeleteTrainScore',
    method: 'delete',
    params: { trainScoreId }
  })
}

// 更新个人训练成绩
export function UpdateTrainScore(data) {
  return request({
    url: '/TrainScore/UpdateTrainScore',
    method: 'put',
    data
  })
}
