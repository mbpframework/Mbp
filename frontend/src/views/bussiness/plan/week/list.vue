<template>
  <div class="app-container">
    <div class="filter-container" style="padding-bottom:10px;">
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
      <el-table-column label="训练日期" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ row.TrainDate| moment("YYYY-MM-DD") }}</span>
        </template>
      </el-table-column>
      <el-table-column label="周几" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ row.DayOfWeek }}</span>
        </template>
      </el-table-column>
      <el-table-column label="时间段" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ row.AmPm }}</span>
        </template>
      </el-table-column>
      <el-table-column label="训练内容" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ row.SubjectContent }}</span>
        </template>
      </el-table-column>
      <el-table-column label="参训对象" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ row.AttendOject }}</span>
        </template>
      </el-table-column>
      <el-table-column label="训练方法" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ row.TrainMethod }}</span>
        </template>
      </el-table-column>
      <el-table-column label="组织者" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ row.Organizer }}</span>
        </template>
      </el-table-column>
      <el-table-column label="质量指标" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ row.QualityIndex }}</span>
        </template>
      </el-table-column>
      <el-table-column label="保障措施" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ row.Safeguards }}</span>
        </template>
      </el-table-column>
      <el-table-column label="课时" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ row.TrainHour }}</span>
        </template>
      </el-table-column>
      <el-table-column label="地址" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ row.Address }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="操作"
        align="center"
        width="100"
        class-name="small-padding fixed-width"
      >
        <template slot-scope="{row}">
          <el-button type="primary" size="mini" @click="handleUpdate(row)">编辑</el-button>
        </template>
      </el-table-column>
    </el-table>

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
            <el-form-item label="训练日期" prop="TrainDate">
              <el-date-picker
                v-model="temp.TrainDate"
                type="date"
                placeholder="选择日期"
                value-format="yyyy-MM-dd"
                style="width:160px"
                readonly
                @change="beginDateChange"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="课时" prop="TrainHour">
              <el-input v-model="temp.TrainHour" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="周几" prop="DayOfWeek">
              <el-input v-model="temp.DayOfWeek" readonly />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="时间段" prop="AmPm">
              <el-input v-model="temp.AmPm" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="训练内容" prop="SubjectIds">
              <el-select
                v-model="temp.SubjectIds"
                multiple
                filterable
                remote
                style="width:410px"
                reserve-keyword
                placeholder="请输入关键词"
                :remote-method="SelectSubjects"
                :loading="loading"
                @change="changeSubjectValue"
              >
                <el-option
                  v-for="item in subjectOptions"
                  :key="item.value"
                  :label="item.label"
                  :value="item.value"
                />
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="参训对象" prop="AttendOject">
              <el-input v-model="temp.AttendOject" type="textarea" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="训练方法" prop="TrainMethod">
              <el-input v-model="temp.TrainMethod" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="组织者" prop="Organizer">
              <el-input v-model="temp.Organizer" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="质量指标" prop="QualityIndex">
              <el-input v-model="temp.QualityIndex" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="保障措施" prop="Safeguards">
              <el-input v-model="temp.Safeguards" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="地址" prop="Address">
              <el-input v-model="temp.Address" />
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
import { GetTrainPlanWeekDetails, UpdateTrainPlanWeekDetail } from '@/api/bll/plan/weekplan'
import waves from '@/directive/waves' // waves directive
import { parseTime } from '@/utils'
import { GetDepts } from '@/api/deptmanage'
import { GetSubjects } from '@/api/subjectmanage'

export default {
  name: 'WeekPlan',
  components: { },
  directives: { waves },
  filters: {
  },
  data() {
    const isNum = (rule, value, callback) => {
      const numberReg = /^\d+$|^\d+[.]?\d+$/
      if (value !== '') {
        if (!numberReg.test(value)) {
          callback(new Error('必须为数字'))
        } else {
          callback()
        }
      } else {
        callback('课时必填')
      }
    }
    return {
      tableKey: 0,
      list: null,
      total: 0,
      listLoading: true,
      loading: true,
      listQuery: {
        pageIndex: 1,
        pageSize: 20,
        DeptName: undefined,
        Title: undefined,
        BeginTime: undefined,
        EndTime: undefined,
        sort: '+Id'
      },
      temp: {
        Id: 0,
        TrainDate: '',
        DayOfWeek: 1,
        AmPm: '上午',
        SubjectContent: '',
        SubjectIds: [],
        AttendOject: '',
        TrainMethod: '',
        Organizer: '',
        QualityIndex: '',
        Safeguards: '',
        TrainHour: 0,
        Address: '',
        EmsTrainPlanWeekId: 0
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑周计划明细',
        create: '新增周计划'
      },
      rules: {
        AmPm: [
          { required: true, message: '时间段必填', trigger: 'change' }
        ],
        SubjectIds: [{ required: true, message: '训练内容必填', trigger: 'change' }],
        AttendOject: [{ required: true, message: '参加对象必填', trigger: 'change' }],
        TrainMethod: [{ required: true, message: '训练方法必填', trigger: 'change' }],
        QualityIndex: [{ required: true, message: '质量指标必填', trigger: 'change' }],
        Safeguards: [{ required: true, message: '保障措施必填', trigger: 'change' }],
        TrainHour: [{ required: true, validator: isNum, trigger: 'change' }],
        Address: [{ required: true, message: '训练地址必填', trigger: 'change' }]
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
      ],
      subjectOptions: [],
      subjectList: [],
      subjectContents: []
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
  mounted() {
    this.GetAllSubject()
  },
  methods: {
    initSubjectContent(subjectIds) {
      this.subjectContents = []
      for (let i = 0; i <= subjectIds.length - 1; i++) {
        this.subjectOptions.find((item) => {
          if (item.value === subjectIds[i]) {
            this.subjectContents.push(item.label)
          }
        })
      }
    },
    changeSubjectValue(val) {
      this.subjectContents = []
      for (let i = 0; i <= val.length - 1; i++) {
        this.subjectOptions.find((item) => {
          if (item.value === val[i]) {
            this.subjectContents.push(item.label)
          }
        })
      }
    },
    GetAllSubject() {
      GetSubjects({ 'pageIndex': 1, 'pageSize': 99999 }).then(response => {
        this.subjectList = response.Data.Content.map(item => {
          return { value: item.Id, label: item.SubjectName }
        })
        this.subjectOptions = this.subjectList
      })
    },
    SelectSubjects(query) {
      this.loading = true
      GetSubjects({ 'pageIndex': 1, 'pageSize': 10, SubjectName: query }).then(response => {
        this.subjectOptions = response.Data.Content.map(item => {
          return { value: item.Id, label: item.SubjectName }
        })
        this.total = response.Data.Total

        // Just to simulate the time of the request
        setTimeout(() => {
          this.loading = false
        }, 100)
      })
    },
    beginDateChange(date) {
      // 结束时间联动5天
      var tempdate = new Date(this.temp.BeginTime)
      this.temp.EndTime = new Date(tempdate.setDate(tempdate.getDate() + 4))
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
      const id = this.$route.params && this.$route.params.id
      GetTrainPlanWeekDetails(id).then(response => {
        this.list = response.Data

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
        TrainDate: '',
        DayOfWeek: 1,
        AmPm: '上午',
        SubjectContent: '',
        SubjectIds: [],
        AttendOject: '',
        TrainMethod: '',
        Organizer: '',
        QualityIndex: '',
        Safeguards: '',
        TrainHour: 0,
        Address: '',
        EmsTrainPlanWeekId: 0
      }
    },
    handleUpdate(row) {
      this.subjectOptions = this.subjectList
      this.initSubjectContent(row.SubjectIds)
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
      const id = this.$route.params && this.$route.params.id
      this.temp.EmsTrainPlanWeekId = id
      this.temp.SubjectContent = this.subjectContents.toString()
      this.$refs['dataForm'].validate(valid => {
        if (valid) {
          const tempData = Object.assign({}, this.temp)
          UpdateTrainPlanWeekDetail(tempData).then(() => {
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
