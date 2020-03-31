<template>
  <div class="app-container">
    <div class="filter-container" style="padding-bottom:10px;">
      <el-input
        v-model="listQuery.Name"
        placeholder="部门名称"
        style="width: 150px;"
        class="filter-item"
        @keyup.enter.native="handleFilter"
      />
      <el-input
        v-model="listQuery.Code"
        placeholder="部门编码"
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
      >新增部门</el-button>
      <el-button
        v-waves
        :loading="downloadLoading"
        class="filter-item"
        type="primary"
        icon="el-icon-download"
        @click="handleDownload"
      >导出部门</el-button>
    </div>

    <el-table
      :data="list"
      style="width: 100%;margin-bottom: 20px;"
      row-key="id"
      border
      :expand-row-keys="expandrowkeys"
      :tree-props="{children: 'children', hasChildren: 'hasChildren'}"
    >
      <el-table-column label="名称" align="center">
        <template slot-scope="{row}">
          <span>{{ row.name }}</span>
        </template>
      </el-table-column>
      <el-table-column label="编码">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ row.DeptCode }}</span>
        </template>
      </el-table-column>
      <el-table-column label="上级部门" align="center">
        <template slot-scope="{row}">
          <span>{{ row.ParentDeptName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="状态" align="center">
        <template slot-scope="{row}">
          <span>{{ row.DeptStatus==1?"激活":"禁用" }}</span>
        </template>
      </el-table-column>
      <el-table-column label="部门全称" align="center">
        <template slot-scope="{row}">
          <span>{{ row.FullDeptName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="顺序" align="center">
        <template slot-scope="{row}">
          <span>{{ row.Order }}</span>
        </template>
      </el-table-column>
      <el-table-column label="父级Id" align="center">
        <template slot-scope="{row}">
          <span>{{ row.ParentId }}</span>
        </template>
      </el-table-column>
      <el-table-column label="操作" align="center" width="230" class-name="small-padding fixed-width">
        <template slot-scope="{row}">
          <el-button v-if="row.Code!='root'" type="primary" size="mini" @click="handleUpdate(row)">编辑</el-button>
          <el-button
            v-if="row.status!='deleted'&&row.ParentId!='0'"
            size="mini"
            type="danger"
            @click="handleDelete(row)"
          >删除</el-button>
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
              <el-input v-model="temp.id" />
            </el-form-item>
            <el-form-item label="名称" prop="name">
              <el-input v-model="temp.name" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="编码" prop="DeptCode">
              <el-input v-model="temp.DeptCode" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="顺序" prop="Order">
              <el-input v-model="temp.Order" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="状态" prop="DeptStatus">
              <el-select v-model="temp.DeptStatus" placeholder="请选择">
                <el-option
                  v-for="item in DeptStatusOptions"
                  :key="item.value"
                  :label="item.label"
                  :value="item.value"
                />
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col v-show="temp.id!=1" :span="12">
            <el-form-item label="父级部门" prop="ParentId">
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
        <el-row v-if="dialogStatus==='update'">
          <el-col :span="24">
            <el-form-item label="部门全称" prop="FullDeptName">
              <el-input v-model="temp.FullDeptName" readonly="readonly" />
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
import { AddDept, UpdateDept, GetDepts, DeleteDept } from '@/api/deptmanage'
import waves from '@/directive/waves' // waves directive
import { parseTime } from '@/utils'
// import Pagination from '@/components/Pagination' // secondary package based on el-pagination
import SelectTree from '@/components/TreeSelect'

export default {
  name: 'DeptManage',
  components: { /* Pagination,*/ SelectTree },
  directives: { waves },
  filters: {},
  data() {
    const validateParentId = (rule, value, callback) => {
      if (this.valueId <= 0) {
        callback(new Error('父级必选'))
      } else {
        callback()
      }
    }
    return {
      tableKey: 0,
      list: [],
      total: 0,
      listLoading: true,
      expandrowkeys: ['1'],
      listQuery: {
        pageIndex: 1,
        pageSize: 999,
        Name: undefined,
        Code: undefined,
        SystemCode: undefined
      },
      SystemCodeOptions: [
        { label: '全部', key: '' }
      ],
      DeptStatusOptions: [{ label: '激活', value: 1 }, { label: '禁用', value: 2 }],
      LevelOptions: [1, 2, 3],
      temp: {
        id: 0,
        name: '',
        DeptCode: '',
        Order: 1,
        ParentId: 0,
        DeptStatus: 0
      },
      isClearable: false, // 可清空（可选）
      isAccordion: true, // 可收起（可选）
      valueId: 1, // 初始ID（可选）
      props: {
        // 配置项（必选）
        value: 'id',
        label: 'name',
        children: 'children'
      },
      menuList: [
      ],
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑部门',
        create: '新增部门'
      },
      rules: {
        name: [{ required: true, message: '名称必填', trigger: 'change' }],
        DeptCode: [{ required: true, message: '编码必填', trigger: 'change' }],
        ParentId: [{ required: true, message: '父级必填', validator: validateParentId, trigger: 'change' }],
        DeptStatus: [{ required: true, message: '状态必填', trigger: 'change' }]
      },
      downloadLoading: false,
      isUpdate: false
    }
  },
  computed: {
    /* 转树形数据 */
    optionData() {
      const cloneData = JSON.parse(JSON.stringify(this.menuList)) // 对源数据深度克隆
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
    this.getList()
    this.getDeptForSelectBox()
  },
  methods: {
    getDeptForSelectBox() {
      GetDepts({ 'pageIndex': 1, 'pageSize': 999 }).then(response => {
        this.menuList = response.Data.Content
      })
    },
    getValue(value) {
      this.valueId = value
      this.temp.ParentId = value
    },
    getList() {
      this.listLoading = true
      GetDepts(this.listQuery).then(response => {
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
    resetTemp() {
      this.temp = {
        id: 0,
        name: '',
        DeptCode: '',
        Order: 1,
        ParentId: 0,
        DeptStatus: 1
      }
    },
    handleCreate() {
      this.getDeptForSelectBox()
      this.resetTemp()
      this.valueId = 1 // 清空给下拉选择框的值
      this.temp.DeptStatus = 1
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
      this.isUpdate = false
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    createData() {
      this.temp.ParentId = this.valueId
      this.$refs['dataForm'].validate(valid => {
        if (valid) {
          AddDept(this.temp).then(response => {
            if (response.Code === 500) return
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
      this.getDeptForSelectBox()
      this.temp = Object.assign({}, row) // copy obj
      this.temp.timestamp = new Date(this.temp.timestamp)
      this.valueId = row.ParentId
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
      this.isUpdate = true
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    updateData() {
      this.$refs['dataForm'].validate(valid => {
        if (valid) {
          const tempData = Object.assign({}, this.temp)
          UpdateDept(tempData).then((response) => {
            if (response.Code === 500) return
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
      DeleteDept(row.id).then(() => {
        this.$notify({
          title: 'Success',
          message: 'Delete Successfully',
          type: 'success',
          duration: 2000
        })
        // const index = this.list.indexOf(row)
        // this.list.splice(index, 1)
        this.handleFilter()
      })
    },
    handleDownload() {
      this.downloadLoading = true
      import('@/vendor/Export2Excel').then(excel => {
        const tHeader = ['timestamp', 'title', 'type', 'importance', 'status']
        const filterVal = [
          'timestamp',
          'title',
          'type',
          'importance',
          'status'
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
      return sort === `+${key}`
        ? 'ascending'
        : sort === `-${key}`
          ? 'descending'
          : ''
    }
  }
}
</script>
