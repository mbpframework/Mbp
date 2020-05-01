import request from '@/utils/request'

// 获取年计划信息
export function GetTrainPlanYears(data) {
  return request({
    url: '/TrainPlanYear/GetTrainPlanYears',
    method: 'get',
    params: { 'pageSize': data.pageSize, 'pageIndex': data.pageIndex, 'Search.DeptName': data.DeptName, 'Search.Title': data.Title,
      'Search.Year': data.Year }
  })
}

// 添加年计划
export function AddTrainPlanYear(data) {
  return request({
    url: '/TrainPlanYear/AddTrainPlanYear',
    method: 'post',
    data
  })
}

// 删除年计划
export function DeleteTrainPlanYear(trainPlanYearId) {
  return request({
    url: '/TrainPlanYear/DeleteTrainPlanYear',
    method: 'delete',
    params: { trainPlanYearId }
  })
}

// 更新年计划
export function UpdateTrainPlanYear(data) {
  return request({
    url: '/TrainPlanYear/UpdateTrainPlanYear',
    method: 'put',
    data
  })
}

