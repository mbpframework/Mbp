import request from '@/utils/request'

// 获取用户
export function GetLogs(data) {
  return request({
    url: '/LogManage/GetLogs',
    method: 'get',
    params: { pageSize: data.pageSize, pageIndex: data.pageIndex,
      'Search.UserId': data.UserId,
      'Search.ClientIP': data.ClientIP,
      'Search.OpDateTimeBegin': data.OpDateTimeBegin,
      'Search.OpDateTimeEnd': data.OpDateTimeEnd,
      'Search.AppName': data.AppName,
      'Search.ModuleName': data.ModuleName,
      'Search.OpName': data.OpName,
      'Search.Desc': data.Desc
    }
  })
}
