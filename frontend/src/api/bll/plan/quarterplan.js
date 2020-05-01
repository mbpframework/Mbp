import request from '@/utils/request'

// 获取季度计划信息
export function GetTrainPlanQuarters(data) {
  return request({
    url: '/TrainPlanQuarter/GetTrainPlanQuarters',
    method: 'get',
    params: { 'pageSize': data.pageSize, 'pageIndex': data.pageIndex, 'Search.DeptName': data.DeptName, 'Search.Title': data.Title,
      'Search.Quarter': data.Quarter }
  })
}

// 添加季度计划
export function AddTrainPlanQuarter(data) {
  return request({
    url: '/TrainPlanQuarter/AddTrainPlanQuarter',
    method: 'post',
    data
  })
}

// 删除季度计划
export function DeleteTrainPlanQuarter(trainPlanQuarterId) {
  return request({
    url: '/TrainPlanQuarter/DeleteTrainPlanQuarter',
    method: 'delete',
    params: { trainPlanQuarterId }
  })
}

// 更新季度计划
export function UpdateTrainPlanQuarter(data) {
  return request({
    url: '/TrainPlanQuarter/UpdateTrainPlanQuarter',
    method: 'put',
    data
  })
}

