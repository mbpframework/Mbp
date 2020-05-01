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
        placeholder="季度计划标题"
        style="width: 150px;"
        class="filter-item"
        @keyup.enter.native="handleFilter"
      />
      <el-select v-model="listQuery.Quarter" placeholder="请选择季度" @change="handleFilter">
        <el-option
          v-for="item in quarterOptions"
          :key="item.value"
          :label="item.label"
          :value="item.value"
        />
      </el-select>
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
      >新增季度计划</el-button>
      <el-button
        v-waves
        :loading="downloadLoading"
        class="filter-item"
        type="primary"
        icon="el-icon-download"
        @click="handleDownload"
      >导出季度计划</el-button>
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
          <span>{{ row.Title }}</span>
        </template>
      </el-table-column>
      <el-table-column label="部门" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ row.DeptName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="季度" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ getQuarterName(row.Quarter) }}</span>
        </template>
      </el-table-column>
      <el-table-column label="备注" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ row.Remark }}</span>
        </template>
      </el-table-column>
      <el-table-column label="附件" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ row.Month }}</span>
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
            <el-form-item label="季度" prop="Quarter">
              <el-select v-model="temp.Quarter" placeholder="请选择季度">
                <el-option
                  v-for="item in quarterOptions"
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
            <el-form-item label="备注" prop="Remark">
              <el-input v-model="temp.Remark" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="附件" prop="AttacheLink">
              <el-upload
                class="upload-demo"
                :data="uploadData"
                :action="uploadUrl"
                :before-upload="handleBeforeUpload"
                :headers="uploadHeads"
                multiple
                :limit="1"
                :on-exceed="handleExceed"
                :file-list="attachList"
              >
                <el-button size="small" type="primary">点击上传</el-button>
                <div slot="tip" class="el-upload__tip">文件不允许超过10MB</div>
              </el-upload>
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
import { AddTrainPlanQuarter, UpdateTrainPlanQuarter, GetTrainPlanQuarters, DeleteTrainPlanQuarter } from '@/api/bll/plan/quarterplan'
import waves from '@/directive/waves' // waves directive
import { parseTime } from '@/utils'
import Pagination from '@/components/Pagination' // secondary package based on el-pagination
import { GetDepts } from '@/api/deptmanage'
import SelectTree from '@/components/TreeSelect'
import { GetFiles } from '@/api/fileupload'
import store from '@/store'
import { getToken } from '@/utils/auth'
import { guid } from '@/utils/uuid'

export default {
  name: 'QuarterPlan',
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
        DeptName: '',
        Title: '',
        Quarter: ''
      },
      temp: {
        Id: 0,
        Title: '',
        DeptId: 1,
        Quarter: '',
        Remark: '',
        AttachmentRelative: ''
      },
      quarterOptions: [{ value: 1, label: '第一季度' }, { value: 2, label: '第二季度' }, { value: 3, label: '第三季度' }, { value: 4, label: '第四季度' }],
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑季度计划',
        create: '新增季度计划'
      },
      rules: {
        DeptId: [
          { required: true, message: '部门必选', validator: validateDept, trigger: 'change' }
        ],
        Quarter: [{ required: true, message: '季度必选', trigger: 'change' }],
        Title: [{ required: true, message: '标题必填', trigger: 'change' }]
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
      uploadUrl: process.env.VUE_APP_BASE_API + '/Attachment/UpLoadFile',
      uploadData: { AttachmentTypeElementCode: 'document', BussinessTypeElementCode: 'TrainPlanQuarter', BussinessId: guid() },
      attachList: [],
      uploadHeads: { Authorization: '' }
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
    },
    getQuarterName() {
      return function(quarter) {
        switch (quarter) {
          case 1: return '第一季度'
          case 2: return '第二季度'
          case 3: return '第三季度'
          case 4: return '第四季度'
          default:return '未知'
        }
      }
    }
  },
  created() {
    this.getDeptForSelectBox()
    this.getList()
  },
  methods: {
    handleBeforeUpload(file) {
      const isLt10M = file.size / 1024 / (1024 * 10) < 1

      if (!isLt10M) {
        this.$message.error('上传文件大小不能超过 10MB!')
      }

      this.uploadData.AttachmentTypeElementCode = 'document'
      this.uploadData.BussinessTypeElementCode = 'TrainPlanQuarter'
      this.uploadData.BussinessId = this.temp.AttachmentRelative
      // 设置上传图片请求的token 以通过验证
      if (store.getters.token) {
        this.uploadHeads.Authorization = 'Bearer ' + getToken()
      }
      return true
    },
    handleExceed(files, fileList) {
      this.$message.warning(`当前限制选择 1 个文件`)
    },
    getAttachmentList() {
      this.listLoading = true
      GetFiles(this.temp.AttachmentRelative).then(response => {
        this.attachList = response.Data.Content
        this.total = response.Data.Total

        // Just to simulate the time of the request
        setTimeout(() => {
          this.listLoading = false
        }, 100)
      })
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
    getList() {
      this.listLoading = true
      GetTrainPlanQuarters(this.listQuery).then(response => {
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
        DeptId: 1,
        Quarter: '',
        Remark: '',
        AttachmentRelative: ''
      }
    },
    handleCreate() {
      this.getDeptForSelectBox()
      this.resetTemp()
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
      this.isUpdate = false
      this.valueId = 1
      this.attachList = []
      // 新增时生成附件关联GUID
      this.temp.AttachmentRelative = guid()
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    createData() {
      this.temp.DeptId = this.valueId
      this.$refs['dataForm'].validate(valid => {
        if (valid) {
          AddTrainPlanQuarter(this.temp).then(() => {
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
      this.getAttachmentList()
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    updateData() {
      this.temp.DeptId = this.valueId
      this.$refs['dataForm'].validate(valid => {
        if (valid) {
          const tempData = Object.assign({}, this.temp)
          UpdateTrainPlanQuarter(tempData).then(() => {
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
      DeleteTrainPlanQuarter(row.Id).then(() => {
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
