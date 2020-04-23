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
      <el-button
        class="filter-item"
        style="margin-left: 10px;"
        type="primary"
        icon="el-icon-edit"
        @click="handleCreate"
      >图表对比</el-button>
    </div>
    <el-table
      :key="tableKey"
      v-loading="listLoading"
      :data="list"
      border
      fit
      highlight-current-row
      style="width: 100%;"
    >
      <el-table-column label="单位" align="center">
        <template slot-scope="{row}">
          <span>{{ row.DeptName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="值机(人次)" align="center">
        <template slot-scope="{row}">
          <span>{{ row.值机 }}</span>
        </template>
      </el-table-column>
      <el-table-column label="值勤业务(人次)" align="center">
        <template slot-scope="{row}">
          <span>{{ row.值勤业务 }}</span>
        </template>
      </el-table-column>
      <el-table-column label="队列(人次)" align="center">
        <template slot-scope="{row}">
          <span>{{ row.队列 }}</span>
        </template>
      </el-table-column>
    </el-table>
    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
      <div id="chartColumn" style="width: 100%; height: 400px;" />
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">取消</el-button>
      </div>
    </el-dialog>
  </div>
</template>
<script>
import waves from '@/directive/waves' // waves directive
import echarts from 'echarts'

export default {
  name: 'DeptTrainReports',
  components: { },
  directives: { waves },
  filters: {},
  data() {
    return {
      tableKey: 0,
      list: [],
      total: 0,
      listLoading: true,
      listQuery: {
        MonthNum: undefined
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        create: '图表'
      },
      chartColumn: null
    }
  },
  created() {
    this.getList()
  },
  mounted() {

  },
  methods: {
    drawLine() {
      this.chartColumn = echarts.init(document.getElementById('chartColumn'))

      this.chartColumn.setOption({
        title: { text: '部门训练人次统计' },
        legend: {},
        tooltip: {},
        dataset: {
          dimensions: ['科目', '值机', '值勤业务', '队列'],
          source: [
            { 科目: '一营', '值机': 43, '值勤业务': 85, '队列': 93 },
            { 科目: '二营', '值机': 83, '值勤业务': 73, '队列': 55 },
            { 科目: '三营', '值机': 86, '值勤业务': 65, '队列': 82 },
            { 科目: '四营', '值机': 72, '值勤业务': 53, '队列': 39 }
          ]
        },
        xAxis: { type: 'category' },
        yAxis: { axisLabel: {
          formatter: '{value} 人次'
        }
        },
        // Declare several bar series, each will be mapped
        // to a column of dataset.source by default.
        series: [
          { type: 'bar' },
          { type: 'bar' },
          { type: 'bar' }
        ]
      })
    },
    getList() {
      this.list = [{ DeptName: '一营', 值机: 43, 值勤业务: 85, 队列: 93 },
        { DeptName: '二营', 值机: 83, 值勤业务: 73, 队列: 55 },
        { DeptName: '三营', 值机: 86, 值勤业务: 65, 队列: 82 },
        { DeptName: '四营', 值机: 72, 值勤业务: 53, 队列: 39 }]
      this.listLoading = false
    },
    handleCreate() {
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
      this.isUpdate = false
      this.$nextTick(function() {
        this.drawLine()
      })
    },
    handleFilter() {
      this.getList()
    }
  }
}
</script>
