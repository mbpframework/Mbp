import request from '@/utils/request'

// 获取月计划信息
export function GetTrainPlanMonths(data) {
  return request({
    url: '/TrainPlanMonth/GetTrainPlanMonths',
    method: 'get',
    params: { 'pageSize': data.pageSize, 'pageIndex': data.pageIndex, 'Search.DeptName': data.DeptName, 'Search.Title': data.Title,
      'Search.Month': data.Month }
  })
}

// 添加月计划
export function AddTrainPlanMonth(data) {
  return request({
    url: '/TrainPlanMonth/AddTrainPlanMonth',
    method: 'post',
    data
  })
}

// 删除月计划
export function DeleteTrainPlanMonth(trainPlanMonthId) {
  return request({
    url: '/TrainPlanMonth/DeleteTrainPlanMonth',
    method: 'delete',
    params: { trainPlanMonthId }
  })
}

// 更新月计划
export function UpdateTrainPlanMonth(data) {
  return request({
    url: '/TrainPlanMonth/UpdateTrainPlanMonth',
    method: 'put',
    data
  })
}

