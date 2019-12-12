<template>
  <div class="app-container">
    <div class="filter-container" style="padding-bottom:10px;">
      <el-input
        v-model="listQuery.Name"
        placeholder="菜单名称"
        style="width: 150px;"
        class="filter-item"
        @keyup.enter.native="handleFilter"
      />
      <el-input
        v-model="listQuery.Code"
        placeholder="菜单编码"
        style="width: 150px;"
        class="filter-item"
        @keyup.enter.native="handleFilter"
      />
      <el-select
        v-model="listQuery.SystemCode"
        placeholder="来源"
        style="width: 150px"
        class="filter-item"
        @change="handleFilter"
      >
        <el-option
          v-for="item in SystemCodeOptions"
          :key="item.key"
          :label="item.label"
          :value="item.key"
        />
      </el-select>
      <el-select
        v-model="listQuery.sort"
        style="width: 140px"
        class="filter-item"
        @change="handleFilter"
      >
        <el-option
          v-for="item in sortOptions"
          :key="item.key"
          :label="item.label"
          :value="item.key"
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
      >新增菜单</el-button>
      <el-button
        v-waves
        :loading="downloadLoading"
        class="filter-item"
        type="primary"
        icon="el-icon-download"
        @click="handleDownload"
      >导出菜单</el-button>
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
        sortable="custom"
        align="center"
        width="80"
        :class-name="getSortClass('Id')"
      >
        <template slot-scope="{row}">
          <span>{{ row.Id }}</span>
        </template>
      </el-table-column>
      <el-table-column label="名称" align="center">
        <template slot-scope="{row}">
          <span>{{ row.Name }}</span>
        </template>
      </el-table-column>
      <el-table-column label="编码">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ row.Code }}</span>
        </template>
      </el-table-column>
      <el-table-column label="顺序" align="center">
        <template slot-scope="{row}">
          <span>{{ row.Order }}</span>
        </template>
      </el-table-column>
      <el-table-column label="层级" align="center">
        <template slot-scope="{row}">
          <span>{{ row.Level }}</span>
        </template>
      </el-table-column>
      <el-table-column label="路径" align="center">
        <template slot-scope="{row}">
          <span>{{ row.Path }}</span>
        </template>
      </el-table-column>
      <el-table-column label="父级Id" align="center">
        <template slot-scope="{row}">
          <span>{{ row.ParentId }}</span>
        </template>
      </el-table-column>
      <el-table-column label="系统编号" align="center">
        <template slot-scope="{row}">
          <span>{{ row.SystemCode }}</span>
        </template>
      </el-table-column>
      <el-table-column label="操作" align="center" width="230" class-name="small-padding fixed-width">
        <template slot-scope="{row}">
          <el-button v-if="row.Code!='root'" type="primary" size="mini" @click="handleUpdate(row)">编辑</el-button>
          <el-button
            v-if="row.status!='deleted'&&row.Code!='root'"
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
      :page.sync="listQuery.page"
      :limit.sync="listQuery.limit"
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
            <el-form-item label="名称" prop="Name">
              <el-input v-model="temp.Name" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="编码" prop="Code">
              <el-input v-model="temp.Code" />
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
            <el-form-item label="父级菜单" prop="ParentId">
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
          <el-col :span="24">
            <el-form-item label="菜单路径" prop="Path">
              <el-input v-model="temp.Path" />
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
import { AddMenu, UpdateMenu, GetMenus, DeleteMenu } from '@/api/menumanage'
import waves from '@/directive/waves' // waves directive
import { parseTime } from '@/utils'
import Pagination from '@/components/Pagination' // secondary package based on el-pagination
import SelectTree from '@/components/TreeSelect'

export default {
  name: 'UserManage',
  components: { Pagination, SelectTree },
  directives: { waves },
  filters: {},
  data() {
    const validateParentId = (rule, value, callback) => {
      if (value <= 0) {
        callback(new Error('父级必选'))
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
        Name: undefined,
        Code: undefined,
        SystemCode: undefined,
        sort: '+Id'
      },
      SystemCodeOptions: [
        { label: '全部', key: '' },
        { label: '数据建模平台', key: 'mdp' },
        { label: '大数据平台', key: 'mbdp' }
      ],
      sortOptions: [
        { label: 'ID升序', key: '+Id' },
        { label: 'ID降序', key: '-Id' }
      ],
      LevelOptions: [1, 2, 3],
      temp: {
        Id: 0,
        Name: '',
        Code: '',
        Order: 1,
        Path: '',
        ParentId: 0
      },
      isClearable: false, // 可清空（可选）
      isAccordion: true, // 可收起（可选）
      valueId: 1, // 初始ID（可选）
      props: {
        // 配置项（必选）
        value: 'Id',
        label: 'Name',
        children: 'children'
      },
      menuList: [
      ],
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑菜单',
        create: '新增菜单'
      },
      rules: {
        Name: [{ required: true, message: '名称必填', trigger: 'change' }],
        Code: [{ required: true, message: '编码必填', trigger: 'change' }],
        ParentId: [{ required: true, message: '父级必填', validator: validateParentId, trigger: 'change' }],
        Path: [{ required: true, message: '路径必填', trigger: 'change' }]
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
          child => father.Id === child.ParentId
        ) // 返回每一项的子级数组
        branchArr.length > 0 ? (father.children = branchArr) : '' // 给父级添加一个children属性，并赋值
        return father.ParentId === 0 // 返回第一层
      })
    }
  },
  created() {
    this.getList()
    this.getMenuForSelectBox()
  },
  methods: {
    getMenuForSelectBox() {
      GetMenus({ 'pageIndex': 1, 'pageSize': 999 }).then(response => {
        this.menuList = response.Data.Content
      })
    },
    getValue(value) {
      this.valueId = value
      this.temp.ParentId = value
    },
    getList() {
      this.listLoading = true
      GetMenus(this.listQuery).then(response => {
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
        Name: '',
        Code: '',
        Order: 1,
        Path: '',
        ParentId: 0
      }
    },
    handleCreate() {
      this.getMenuForSelectBox()
      this.resetTemp()
      this.valueId = 1 // 清空给下拉选择框的值
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
      this.isUpdate = false
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    createData() {
      this.temp.ParentId = 0
      this.$refs['dataForm'].validate(valid => {
        if (valid) {
          AddMenu(this.temp).then(() => {
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
      this.getMenuForSelectBox()
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
          // const that = this
          UpdateMenu(tempData).then(() => {
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
      DeleteMenu(row.Id).then(() => {
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
