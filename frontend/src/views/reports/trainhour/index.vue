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
      show-summary
      border

      highlight-current-row
      style="width: 100%;"
    >
      <el-table-column label="单位" align="center">
        <template slot-scope="{row}">
          <span>{{ row.DeptName }}</span>
        </template>
      </el-table-column>
      <el-table-column sortable label="第一周" prop="First" align="center">
        <template slot-scope="{row}">
          {{ row.First }}
        </template>
      </el-table-column>
      <el-table-column sortable label="第二周" prop="Sencond" align="center">
        <template slot-scope="{row}">
          {{ row.Sencond }}
        </template>
      </el-table-column>
      <el-table-column sortable label="第三周" prop="Third" align="center">
        <template slot-scope="{row}">
          {{ row.Third }}
        </template>
      </el-table-column>
      <el-table-column sortable label="第四周" prop="Forth" align="center">
        <template slot-scope="{row}">
          {{ row.Forth }}
        </template>
      </el-table-column>
      <el-table-column sortable label="合计" prop="Total" align="center">
        <template slot-scope="{row}">
          {{ row.First+row.Sencond+row.Third+row.Forth }}
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script>
import { } from '@/api/bll/plan/weekplan'
import waves from '@/directive/waves' // waves directive
import { parseTime } from '@/utils'

export default {
  name: 'WeekPlan',
  components: { },
  directives: { waves },
  filters: {
  },
  data() {
    return {
      tableKey: 0,
      list: [{ DeptName: '一营', First: 100, Sencond: 159, Third: 136, Forth: 153 },
        { DeptName: '二营', First: 153, Sencond: 189, Third: 144, Forth: 142 },
        { DeptName: '三营', First: 231, Sencond: 174, Third: 151, Forth: 138 }],
      total: 0,
      listLoading: true,
      listQuery: {
        MonthNum: undefined
      },
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
      },
      downloadLoading: false

    }
  },
  computed: {
  },
  created() {
    this.getList()
  },
  methods: {
    getList() {
      this.list = [
        { DeptName: '一营', First: 100, Sencond: 159, Third: 136, Forth: 153 },
        { DeptName: '二营', First: 153, Sencond: 189, Third: 144, Forth: 142 },
        { DeptName: '三营', First: 231, Sencond: 174, Third: 151, Forth: 138 }
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
    }
  }
}
</script>
