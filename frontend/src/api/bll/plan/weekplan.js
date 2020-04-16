import request from '@/utils/request'

// 获取周计划信息
export function GetTrainPlanWeeks(data) {
  return request({
    url: '/TrainPlan/GetTrainPlanWeeks',
    method: 'get',
    params: { 'pageSize': data.pageSize, 'pageIndex': data.pageIndex, 'Search.DeptName': data.DeptName, 'Search.Title': data.Title,
      'Search.BeginTime': data.BeginTime, 'Search.EndTime': data.EndTime }
  })
}

// 获取周计划明细信息
export function GetTrainPlanWeekDetails(trainPlanWeekId) {
  return request({
    url: '/TrainPlan/GetTrainPlanWeekDetails',
    method: 'get',
    params: { 'trainPlanWeekId': trainPlanWeekId }
  })
}

// 添加周计划
export function AddTrainPlanWeek(data) {
  return request({
    url: '/TrainPlan/AddTrainPlanWeek',
    method: 'post',
    data
  })
}

// 删除周计划
export function DeleteTrainPlanWeek(trainPlanWeekId) {
  return request({
    url: '/TrainPlan/DeleteTrainPlanWeek',
    method: 'delete',
    params: { trainPlanWeekId }
  })
}

// 更新周计划
export function UpdateTrainPlanWeek(data) {
  return request({
    url: '/TrainPlan/UpdateTrainPlanWeek',
    method: 'put',
    data
  })
}

// 更新周计划明细
export function UpdateTrainPlanWeekDetail(data) {
  return request({
    url: '/TrainPlan/UpdateTrainPlanWeekDetail',
    method: 'put',
    data
  })
}
