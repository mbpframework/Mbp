<template>
  <div class="app-container">
    <div class="filter-container" style="padding-bottom:10px;">
      <el-input
        v-model="listQuery.Title"
        placeholder="公告标题"
        style="width: 150px;"
        class="filter-item"
        @keyup.enter.native="handleFilter"
      />
      <el-date-picker
        v-model="listQuery.PublishTime"
        type="datetime"
        placeholder="发布时间"
        @change="handleFilter"
      />
      <el-select v-model="listQuery.NoticeType" placeholder="公告类型" @change="handleFilter">
        <el-option
          v-for="item in noticeTypeOptions"
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
      >查询</el-button></div>

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
          <span>{{ row.NoticeTitle }}</span>
        </template>
      </el-table-column>
      <el-table-column label="发布时间" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ row.PublishTime }}</span>
        </template>
      </el-table-column>
      <el-table-column label="类型" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ getNoticeType(row.NoticeType) }}</span>
        </template>
      </el-table-column>
      <el-table-column label="状态" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ getNoticeStatus(row.NoticeStatus) }}</span>
        </template>
      </el-table-column>
      <el-table-column label="备注" align="center">
        <template slot-scope="{row}">
          <span>{{ row.Remark }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="操作"
        align="center"
        width="270"
        class-name="small-padding fixed-width"
      >
        <template slot-scope="{row}">
          <el-button type="primary" size="mini" @click="handleUpdate(row)">详情</el-button>
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
        :model="temp"
        label-position="right"
        label-width="90px"
        style="width: 650px; margin-left:50px;"
      >
        <el-row>
          <el-col :span="24">
            <el-form-item v-show="false" label="ID" prop="Id">
              <el-input v-model="temp.Id" />
            </el-form-item>
            <el-form-item label="标题" prop="NoticeTitle">
              <el-input v-model="temp.NoticeTitle" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="类型" prop="NoticeType">
              <el-select v-model="temp.NoticeType" placeholder="请选择" style="width:220px;">
                <el-option
                  v-for="item in noticeTypeOptions"
                  :key="item.value"
                  :label="item.label"
                  :value="item.value"
                />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="发布时间" prop="PublishTime">
              <el-date-picker
                v-model="temp.PublishTime"
                type="datetime"
                placeholder="发布时间"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="内容" prop="NoticeContent">
              <el-input v-model="temp.NoticeContent" type="textarea" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="附件" prop="AttacheLink">
              <el-upload
                class="upload-demo"
                :action="uploadUrl"
                multiple
                :limit="1"
                :on-preview="fileOnclick"
                :file-list="attachList"
              >
                <el-button size="small" type="primary">点击上传</el-button>
                <div slot="tip" class="el-upload__tip">文件不允许超过10MB</div>
              </el-upload>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="备注" prop="Remark">
              <el-input v-model="temp.Remark" type="textarea" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">取消</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import { GetNotices, DeleteNotice } from '@/api/bll/notice/noticemanage'
import waves from '@/directive/waves' // waves directive
import { parseTime } from '@/utils'
import Pagination from '@/components/Pagination' // secondary package based on el-pagination
import { GetFiles } from '@/api/fileupload'
import { guid } from '@/utils/uuid'

export default {
  name: 'NoticeManage',
  components: { Pagination },
  directives: { waves },
  filters: {},
  data() {
    return {
      tableKey: 0,
      list: [],
      total: 0,
      listLoading: true,
      listQuery: {
        pageIndex: 1,
        pageSize: 20,
        Title: undefined,
        PublishTime: undefined,
        NoticeType: undefined,
        NoticeStatus: 2
      },
      noticeTypeOptions: [{ label: '公告', value: 1 },
        { label: '通告', value: 2 },
        { label: '决议', value: 3 },
        { label: '公报', value: 4 },
        { label: '意见', value: 5 },
        { label: '通报', value: 6 },
        { label: '纪要', value: 7 }],
      noticeStatusOptions: [{ label: '草稿', value: 1 },
        { label: '发布', value: 2 }],
      temp: {
        Id: 0,
        NoticeTitle: '',
        NoticeContent: '',
        PublishTime: '',
        NoticeType: 1,
        NoticeStatus: 1,
        AttachmentRelative: '',
        Remark: ''
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '查看文件信息',
        create: '新增文件信息'
      },
      downloadLoading: false,
      isUpdate: false,
      uploadUrl: process.env.VUE_APP_BASE_API + '/Attachment/UpLoadFile',
      uploadData: { AttachmentTypeElementCode: 'document', BussinessTypeElementCode: 'Notice', BussinessId: guid() },
      attachList: [],
      uploadHeads: { Authorization: '' }
    }
  },
  computed: {
    getNoticeType() {
      return function(type) {
        switch (type) {
          case 1: return '公告'
          case 2: return '通告'
          case 3: return '决议'
          case 4: return '公报'
          case 5: return '意见'
          case 6: return '通报'
          case 7: return '纪要'
          default:return '未知'
        }
      }
    },
    getNoticeStatus() {
      return function(status) {
        switch (status) {
          case 1: return '草稿'
          case 2: return '发布'
          default:return '未知'
        }
      }
    }
  },
  created() {
    this.getList()
  },
  methods: {
    fileOnclick(file) {
      // 下载文件
      const url = process.env.VUE_APP_BASE_API + '/Attachment/FetchAttachment?fileName=' + file.name + '&url=' + file.url
      window.open(url, '_blank')
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
    getList() {
      this.listLoading = true
      GetNotices(this.listQuery).then(response => {
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
      row.NoticeStatus = status
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
        NoticeTitle: '',
        NoticeContent: '',
        PublishTime: '',
        NoticeType: 1,
        NoticeStatus: 1,
        AttachmentRelative: '',
        Remark: ''
      }
    },
    handleUpdate(row) {
      this.temp = Object.assign({}, row) // copy obj
      this.temp.timestamp = new Date(this.temp.timestamp)
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
      this.isUpdate = true
      this.getAttachmentList()
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    handleDelete(row) {
      DeleteNotice(row.Id).then(() => {
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
