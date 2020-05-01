<template>
  <div class="app-container">
    <div class="filter-container" style="padding-bottom:10px;">
      <el-date-picker
        v-model="listQuery.MonthNum"
        type="month"
        placeholder="选择月份"
        @keyup.enter.native="handleFilter"
      />
      <el-button
        v-waves
        class="filter-item"
        type="primary"
        icon="el-icon-search"
        @click="handleFilter"
      >查询</el-button>
    </div>

    <el-table
      :key="tableKey"
      v-loading="listLoading"
      :data="list"
      border
      fit
      highlight-current-row
      style="width: 100%;"
      @sort-change="sortChange"
    >
      <el-table-column label="单位" align="center">
        <template slot-scope="{row}">
          <span>{{ row.DeptName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="应训人数" align="center">
        <template slot-scope="{row}">
          <span>{{ row.TrainNumber }}</span>
        </template>
      </el-table-column>
      <el-table-column label="实训人数" align="center">
        <template slot-scope="{row}">
          <span>{{ row.AttendNumber }}</span>
        </template>
      </el-table-column>
      <el-table-column label="参训率" align="center">
        <template slot-scope="{row}">
          <span>{{ row.AttendRate }}</span>
        </template>
      </el-table-column>
      <el-table-column label="平均参训时长" align="center">
        <template slot-scope="{row}">
          <span>{{ row.AvgTrainHour }}</span>
        </template>
      </el-table-column>
      <el-table-column label="训练计划完成率" align="center">
        <template slot-scope="{row}">
          <span>{{ row.TrainPlanRate }}</span>
        </template>
      </el-table-column>
      <el-table-column label="参加考核总数" align="center">
        <template slot-scope="{row}">
          <span>{{ row.AttendExamNumber }}</span>
        </template>
      </el-table-column>
      <el-table-column label="人均参考次数" align="center">
        <template slot-scope="{row}">
          <span>{{ row.AvgAttendExamNumber }}</span>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script>
import { } from '@/api/bll/plan/weekplan'
import waves from '@/directive/waves' // waves directive
import { parseTime } from '@/utils'
import { GetDepts } from '@/api/deptmanage'

export default {
  name: 'WeekPlan',
  components: { },
  directives: { waves },
  filters: {
  },
  data() {
    const validateDept = (rule, value, callback) => {
      if (this.valueId <= 0) {
        callback(new Error('部门必选'))
      } else {
        callback()
      }
    }
    return {
      tableKey: 0,
      list: null,
      total: 0,
      listLoading: true,
      listQuery: {
        MonthNum: undefined
      },
      trainTypeOptions: [{ label: '军官共同训练', value: 1 },
        { label: '士兵共同训练', value: 2 },
        { label: '光端专业训练', value: 3 },
        { label: '军官专业训练', value: 4 },
        { label: '通信员专业训练', value: 5 },
        { label: '光端战术训练', value: 6 },
        { label: '营连战术训练', value: 7 },
        { label: '部队训练', value: 8 }],
      temp: {
        Id: 0,
        Title: '',
        BeginTime: undefined,
        EndTime: undefined,
        DeptId: 1,
        WeekNum: 1,
        Month: 1
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑周计划',
        create: '新增周计划'
      },
      rules: {
        DeptId: [
          { required: true, message: '部门必选', validator: validateDept, trigger: 'change' }
        ],
        BeginTime: [{ required: true, message: '开始时间必填', trigger: 'change' }],
        EndTime: [{ required: true, message: '结束时间必填', trigger: 'change' }],
        Title: [{ required: true, message: '标题', trigger: 'change' }]
      },
      downloadLoading: false,
      isUpdate: false,
      isClearable: false, // 可清空（可选）
      isAccordion: true, // 可收起（可选）
      valueId: 1, // 初始ID（可选）
      placeholder: '请选择部门',
      props: {
        // 配置项（必选）
        value: 'id',
        label: 'name',
        children: 'children'
      },
      deptList: [
      ]
    }
  },
  computed: {
    optionData() {
      const cloneData = JSON.parse(JSON.stringify(this.deptList)) // 对源数据深度克隆
      return cloneData.filter(father => {
        // 循环所有项，并添加children属性
        const branchArr = cloneData.filter(
          child => father.id === child.ParentId
        ) // 返回每一项的子级数组
        branchArr.length > 0 ? (father.children = branchArr) : '' // 给父级添加一个children属性，并赋值
        return father.ParentId === 0 // 返回第一层
      })
    }
  },
  created() {
    this.getDeptForSelectBox()
    this.getList()
  },
  methods: {
    beginDateChange(date) {
      // 结束时间联动5天
      var tempdate = new Date(this.temp.BeginTime)
      this.temp.EndTime = new Date(tempdate.setDate(tempdate.getDate() + 4))
    },
    EndTimeSure(date) {
      // 默认给定第几月
      this.temp.Month = new Date(date).getMonth() + 1
    },
    getDeptForSelectBox() {
      GetDepts({ 'pageIndex': 1, 'pageSize': 999 }).then(response => {
        this.deptList = response.Data.Content
      })
    },
    getValue(value) {
      this.valueId = value
    },
    getSearchValue(value) {
      this.valueSearchId = value
    },
    selectPosition(value) {
      this.listQuery.PositionId = value
      this.handleFilter()
    },
    getList() {
      this.list = [
        { DeptName: '一营', TrainNumber: 100, AttendNumber: 78, AttendRate: '78%', AvgTrainHour: 58.6, TrainPlanRate: '80%', AttendExamNumber: 100, AvgAttendExamNumber: 2 },
        { DeptName: '二营', TrainNumber: 100, AttendNumber: 75, AttendRate: '75%', AvgTrainHour: 55.6, TrainPlanRate: '80%', AttendExamNumber: 100, AvgAttendExamNumber: 2 },
        { DeptName: '三营', TrainNumber: 100, AttendNumber: 80, AttendRate: '80%', AvgTrainHour: 60.6, TrainPlanRate: '80%', AttendExamNumber: 100, AvgAttendExamNumber: 2 }
      ]
      // this.listLoading = true
      // GetTrainPlanWeeks(this.listQuery).then(response => {
      //   this.list = response.Data.Content
      //   this.total = response.Data.Total

      // Just to simulate the time of the request
      setTimeout(() => {
        this.listLoading = false
      }, 100)
      // })
    },
    handleFilter() {
      this.listQuery.pageIndex = 1
      this.getList()
    },
    handleModifyStatus(row, status) {
      this.$message({
        message: '操作Success',
        type: 'success'
      })
      row.status = status
    },
    sortChange(data) {
      const { prop, order } = data
      if (prop === 'Id') {
        this.sortByID(order)
      }
    },
    sortByID(order) {
      if (order === 'ascending') {
        this.listQuery.sort = '+Id'
      } else {
        this.listQuery.sort = '-Id'
      }
      this.handleFilter()
    },
    resetTemp() {
      this.temp = {
        Id: 0,
        Title: '',
        BeginTime: undefined,
        EndTime: undefined,
        DeptId: 1,
        WeekNum: 1,
        Month: 1
      }
    },
    handleDownload() {
      this.downloadLoading = true
      import('@/vendor/Export2Excel').then(excel => {
        const tHeader = ['Id', 'Name', 'Code', 'SystemCode']
        const filterVal = [
          'Id',
          'Name',
          'Code',
          'SystemCode'
        ]
        const data = this.formatJson(filterVal, this.list)
        excel.export_json_to_excel({
          header: tHeader,
          data,
          filename: 'table-list'
        })
        this.downloadLoading = false
      })
    },
    formatJson(filterVal, jsonData) {
      return jsonData.map(v =>
        filterVal.map(j => {
          if (j === 'timestamp') {
            return parseTime(v[j])
          } else {
            return v[j]
          }
        })
      )
    },
    getSortClass: function(key) {
      const sort = this.listQuery.sort
      return sort === `+${key}` ? 'ascending' : sort === `-${key}` ? 'descending' : ''
    }
  }
}
</script>
