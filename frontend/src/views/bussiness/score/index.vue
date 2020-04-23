<template>
  <div class="app-container">
    <div class="filter-container" style="padding-bottom:10px;">
      <el-input
        v-model="listQuery.UserName"
        placeholder="姓名"
        style="width: 150px;"
        class="filter-item"
        @keyup.enter.native="handleFilter"
      />
      <el-input
        v-model="listQuery.Major"
        placeholder="专业"
        style="width: 150px;"
        class="filter-item"
        @keyup.enter.native="handleFilter"
      />
      <el-select
        v-model="listQuery.SubjectId"
        filterable
        clearable
        remote
        reserve-keyword
        placeholder="科目(模糊)"
        :remote-method="SelectSubject"
        :loading="loading"
        @change="handleFilter"
      >
        <el-option
          v-for="item in subjectOptions"
          :key="item.value"
          :label="item.label"
          :value="item.value"
        />
      </el-select>
      <el-date-picker
        v-model="listQuery.TrainDate"
        type="date"
        placeholder="训练日期"
        value-format="yyyy-MM-dd"
        style="width:160px"
      />
      <el-input
        v-model="listQuery.Score"
        placeholder="成绩"
        style="width: 150px;"
        class="filter-item"
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
      >新增个人成绩</el-button>
      <el-button
        v-waves
        :loading="downloadLoading"
        class="filter-item"
        type="primary"
        icon="el-icon-download"
        @click="handleDownload"
      >导出个人成绩</el-button>
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
      <el-table-column label="姓名" align="center">
        <template slot-scope="{row}">
          <span>{{ row.UserName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="登录名" align="center">
        <template slot-scope="{row}">
          <span>{{ row.LoginName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="专业" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ row.Major }}</span>
        </template>
      </el-table-column>
      <el-table-column label="科目" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ row.SubjectName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="训练日期" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ row.TrainDate| moment("YYYY-MM-DD") }}</span>
        </template>
      </el-table-column>
      <el-table-column label="训练课时" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ row.TrainHour }}</span>
        </template>
      </el-table-column>
      <el-table-column label="分数" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ row.Score }}</span>
        </template>
      </el-table-column>
      <el-table-column label="备注" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ row.Remark }}</span>
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
            <el-form-item label="登录名" prop="LoginName">
              <el-input v-model="temp.LoginName" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="科目" prop="SubjectId">
              <el-select
                v-model="temp.SubjectId"
                filterable
                clearable
                remote
                reserve-keyword
                placeholder="科目(模糊)"
                :remote-method="SelectSubject"
                :loading="loading"
                @change="handleFilter"
              >
                <el-option
                  v-for="item in subjectOptions"
                  :key="item.value"
                  :label="item.label"
                  :value="item.value"
                />
              </el-select></el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="训练日期" prop="TrainDate">
              <el-date-picker
                v-model="temp.TrainDate"
                type="date"
                placeholder="选择日期"
                value-format="yyyy-MM-dd"
                style="width:160px"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="专业" prop="Major">
              <el-input v-model="temp.Major" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="训练课时" prop="TrainHour">
              <el-input v-model="temp.TrainHour" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="成绩" prop="Score">
              <el-input v-model="temp.Score" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="备注" prop="Remark">
              <el-input v-model="temp.Remark" />
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
import { AddTrainScore, UpdateTrainScore, GetTrainScores, DeleteTrainScore } from '@/api/bll/score/scoremanage'
import waves from '@/directive/waves' // waves directive
import { parseTime } from '@/utils'
import Pagination from '@/components/Pagination' // secondary package based on el-pagination
import { GetSubjects } from '@/api/subjectmanage'

export default {
  name: 'TrainScore',
  components: { Pagination },
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
      loading: true,
      listLoading: true,
      listQuery: {
        pageIndex: 1,
        pageSize: 20,
        UserName: '',
        Major: '',
        SubjectId: '',
        TrainDate: undefined,
        Score: ''
      },
      temp: {
        Id: 0,
        Major: '',
        LoginName: '',
        SubjectId: '',
        TrainDate: undefined,
        TrainHour: 0,
        Score: 0,
        Remark: ''
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑个人成绩',
        create: '新增个人成绩'
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
      subjectOptions: []
    }
  },
  computed: {
  },
  created() {
    this.getList()
  },
  methods: {
    SelectSubject(query) {
      this.loading = true
      GetSubjects({ 'pageIndex': 1, 'pageSize': 6, SubjectName: query }).then(response => {
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
    getValue(value) {
      this.valueId = value
    },
    getList() {
      this.listLoading = true
      GetTrainScores(this.listQuery).then(response => {
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
        Major: '',
        LoginName: '',
        SubjectId: '',
        TrainDate: undefined,
        TrainHour: 0,
        Score: 0,
        Remark: ''
      }
    },
    handleCreate() {
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
          AddTrainScore(this.temp).then(() => {
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
          UpdateTrainScore(tempData).then(() => {
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
      DeleteTrainScore(row.Id).then(() => {
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
