<template>
  <div class="app-container">
    <div class="filter-container" style="padding-bottom:10px;">
      <el-input
        v-model="listQuery.DeptName"
        placeholder="部门名称"
        style="width: 150px;"
        class="filter-item"
        @keyup.enter.native="handleFilter"
      />
      <el-input
        v-model="listQuery.Title"
        placeholder="周训练标题"
        style="width: 150px;"
        class="filter-item"
        @keyup.enter.native="handleFilter"
      />
      <el-date-picker
        v-model="listQuery.BeginTime"
        type="date"
        placeholder="开始日期"
        value-format="yyyy-MM-dd"
        style="width:160px"
      />
      <el-date-picker
        v-model="listQuery.EndTime"
        type="date"
        placeholder="结束日期"
        value-format="yyyy-MM-dd"
        style="width:160px"
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
      >新增周计划</el-button>
      <el-button
        v-waves
        :loading="downloadLoading"
        class="filter-item"
        type="primary"
        icon="el-icon-download"
        @click="handleDownload"
      >导出周计划</el-button>
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
      <el-table-column
        label="ID"
        prop="id"
        align="center"
        width="80"
      >
        <template slot-scope="{row}">
          <span>{{ row.Id }}</span>
        </template>
      </el-table-column>
      <el-table-column label="标题" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)"> {{ row.Title }}</span>
        </template>
      </el-table-column>
      <el-table-column label="部门" align="center">
        <template slot-scope="{row}">
          <span>{{ row.DeptName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="月份" align="center">
        <template slot-scope="{row}">
          <span>{{ row.Month }}</span>
        </template>
      </el-table-column>
      <el-table-column label="周数" align="center">
        <template slot-scope="{row}">
          <span>{{ row.WeekNum }}</span>
        </template>
      </el-table-column>
      <el-table-column label="开始时间" align="center">
        <template slot-scope="{row}">
          <span>{{ row.BeginTime| moment("YYYY-MM-DD") }}</span>
        </template>
      </el-table-column>
      <el-table-column label="结束时间" align="center">
        <template slot-scope="{row}">
          <span>{{ row.EndTime| moment("YYYY-MM-DD") }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="操作"
        align="center"
        width="270"
        class-name="small-padding fixed-width"
      >
        <template slot-scope="{row}">
          <el-button type="primary" size="mini" @click="handleUpdate(row)">编辑</el-button>
          <router-link :to="'/bussiness/plan/week/list/'+row.Id">
            <el-button type="primary" size="mini">
              明细
            </el-button>
          </router-link>
          <el-button
            v-if="row.status!='deleted'"
            size="mini"
            type="danger"
            @click="handleDelete(row)"
          >删除</el-button>
        </template>
      </el-table-column>
    </el-table>

    <pagination
      v-show="total>0"
      :total="total"
      :page.sync="listQuery.pageIndex"
      :limit.sync="listQuery.pageSize"
      @pagination="getList"
    />

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
      <el-form
        ref="dataForm"
        :rules="rules"
        :model="temp"
        label-position="right"
        label-width="90px"
        style="width: 500px; margin-left:50px;"
      >
        <el-row>
          <el-col :span="12">
            <el-form-item v-show="false" label="ID" prop="Id">
              <el-input v-model="temp.Id" />
            </el-form-item>
            <el-form-item label="标题" prop="Title">
              <el-input v-model="temp.Title" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="部门" prop="DeptId">
              <SelectTree
                :props="props"
                :options="optionData"
                :value="valueId"
                :clearable="isClearable"
                :accordion="isAccordion"
                @getValue="getValue($event)"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="开始时间" prop="BeginTime">
              <el-date-picker
                v-model="temp.BeginTime"
                type="date"
                placeholder="选择日期"
                value-format="yyyy-MM-dd"
                style="width:160px"
                @change="beginDateChange"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="结束时间" prop="EndTime">
              <el-date-picker
                v-model="temp.EndTime"
                type="date"
                placeholder="选择日期"
                value-format="yyyy-MM-dd"
                style="width:160px"
                @change="EndTimeSure"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="月份" prop="Month">
              <el-input v-model="temp.Month" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="周数" prop="WeekNum">
              <el-input v-model="temp.WeekNum" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">取消</el-button>
        <el-button type="primary" @click="dialogStatus==='create'?createData():updateData()">确认</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import { AddTrainPlanWeek, UpdateTrainPlanWeek, GetTrainPlanWeeks, DeleteTrainPlanWeek } from '@/api/bll/plan/weekplan'
import waves from '@/directive/waves' // waves directive
import { parseTime } from '@/utils'
import Pagination from '@/components/Pagination' // secondary package based on el-pagination
import { GetDepts } from '@/api/deptmanage'
import SelectTree from '@/components/TreeSelect'

export default {
  name: 'WeekPlan',
  components: { Pagination, SelectTree },
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
        pageIndex: 1,
        pageSize: 20,
        DeptName: undefined,
        Title: undefined,
        BeginTime: undefined,
        EndTime: undefined,
        sort: '+Id'
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
      this.listLoading = true
      GetTrainPlanWeeks(this.listQuery).then(response => {
        this.list = response.Data.Content
        this.total = response.Data.Total

        // Just to simulate the time of the request
        setTimeout(() => {
          this.listLoading = false
        }, 100)
      })
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
    handleCreate() {
      this.getDeptForSelectBox()
      this.resetTemp()
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
      this.isUpdate = false
      this.valueId = 1
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    createData() {
      this.temp.DeptId = this.valueId
      this.$refs['dataForm'].validate(valid => {
        if (valid) {
          AddTrainPlanWeek(this.temp).then(() => {
            this.list.unshift(this.temp)
            this.dialogFormVisible = false
            this.$notify({
              title: 'Success',
              message: '新增成功',
              type: 'success',
              duration: 2000
            })
            this.handleFilter()
          })
        }
      })
    },
    handleUpdate(row) {
      this.temp = Object.assign({}, row) // copy obj
      this.temp.timestamp = new Date(this.temp.timestamp)
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
      this.isUpdate = true
      this.valueId = row.DeptId
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    updateData() {
      this.temp.DeptId = this.valueId
      this.$refs['dataForm'].validate(valid => {
        if (valid) {
          const tempData = Object.assign({}, this.temp)
          UpdateTrainPlanWeek(tempData).then(() => {
            for (const v of this.list) {
              if (v.Id === this.temp.Id) {
                const index = this.list.indexOf(v)
                this.list.splice(index, 1, this.temp)
                break
              }
            }
            this.dialogFormVisible = false
            this.$notify({
              title: 'Success',
              message: 'Update Successfully',
              type: 'success',
              duration: 2000
            })
            this.handleFilter()
          })
        }
      })
    },
    handleDelete(row) {
      DeleteTrainPlanWeek(row.Id).then(() => {
        this.$notify({
          title: 'Success',
          message: 'Delete Successfully',
          type: 'success',
          duration: 2000
        })
        this.handleFilter()
        // const index = this.list.indexOf(row)
        // this.list.splice(index, 1)
      })
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
