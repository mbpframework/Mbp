<template>
  <div class="app-container">
    <div class="filter-container" style="padding-bottom:10px;">
      <el-input
        v-model="listQuery.RoleName"
        placeholder="角色名称"
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
      >新增角色</el-button>
      <el-button
        v-waves
        :loading="downloadLoading"
        class="filter-item"
        type="primary"
        icon="el-icon-download"
        @click="handleDownload"
      >导出角色</el-button>
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
      <el-table-column label="角色名" align="center">
        <template slot-scope="{row}">
          <span>{{ row.Name }}</span>
        </template>
      </el-table-column>
      <el-table-column label="编码" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ row.Code }}</span>
        </template>
      </el-table-column>
      <el-table-column label="系统编号" align="center">
        <template slot-scope="{row}">
          <span>{{ row.SystemCode }}</span>
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
          <el-button type="primary" size="mini" @click="handleRoleGrant(row)">编辑权限</el-button>
          <el-button
            v-if="row.status!='deleted'"
            size="mini"
            type="danger"
            @click="handleModifyStatus(row,'deleted')"
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
        style="width: 400px; margin-left:50px;"
      >
        <el-row>
          <el-col :span="12">
            <el-form-item v-show="false" label="ID" prop="Id">
              <el-input v-model="temp.Id" />
            </el-form-item>
            <el-form-item label="角色名" prop="Name">
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
          <el-col :span="24">
            <el-form-item label="系统编码" prop="SystemCode">
              <el-select v-model="temp.SystemCode" placeholder="请选择系统">
                <el-option
                  v-for="item in SystemEditCodeOptions"
                  :key="item.key"
                  :label="item.label"
                  :value="item.key"
                />
              </el-select>
              <!-- <el-input v-model="temp.SystemCode" /> -->
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">取消</el-button>
        <el-button type="primary" @click="dialogStatus==='create'?createData():updateData()">确认</el-button>
      </div>
    </el-dialog>
    <el-dialog :visible.sync="dialogPermissionVisible" title="角色授权">
      <el-form :model="roleGrantModel" label-width="80px" label-position="left">
        <el-form-item label="角色名称">
          <el-input v-model="roleGrantModel.Name" readonly />
        </el-form-item>
        <el-form-item label="角色编码">
          <el-input v-model="roleGrantModel.Code" readonly />
        </el-form-item>
        <el-form-item label="角色功能">
          <el-tree
            ref="tree"
            :data="roleMenus"
            :default-checked-keys="checkedMenus"
            :props="defaultProps"
            show-checkbox
            default-expand-all
            node-key="Id"
            class="permission-tree"
          />
        </el-form-item>
      </el-form>
      <div style="text-align:right;">
        <el-button @click="dialogPermissionVisible=false">取消</el-button>
        <el-button type="primary" @click="GrantRole(roleGrantModel.Id)">确认</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import { AddRole, UpdateRole, GetRoles, AddRoleMenus, GetRoleMenus } from '@/api/rolemanage'
import { GetMenus } from '@/api/menumanage'
import waves from '@/directive/waves' // waves directive
import { parseTime } from '@/utils'
import Pagination from '@/components/Pagination' // secondary package based on el-pagination

export default {
  name: 'RoleManage',
  components: { Pagination },
  directives: { waves },
  filters: {},
  data() {
    return {
      tableKey: 0,
      list: null,
      total: 0,
      listLoading: true,
      listQuery: {
        pageIndex: 1,
        pageSize: 20,
        SystemCode: undefined,
        RoleName: undefined,
        sort: '+Id'
      },
      SystemCodeOptions: [
        { label: '全部', key: '' },
        { label: '数据建模平台', key: 'mdp' },
        { label: '大数据平台', key: 'mbdp' }
      ],
      SystemEditCodeOptions: [
        { label: '全部', key: '' },
        { label: '数据建模平台', key: 'mdp' },
        { label: '大数据平台', key: 'mbdp' }
      ],
      sortOptions: [
        { label: 'ID升序', key: '+Id' },
        { label: 'ID降序', key: '-Id' }
      ],
      temp: {
        Id: 0,
        Name: '',
        Code: ''
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑角色',
        create: '新增角色'
      },
      rules: {
        Name: [
          { required: true, message: '角色名必填', trigger: 'change' }
        ],
        Code: [{ required: true, message: '编码必填', trigger: 'change' }],
        SystemCode: [{ required: true, message: '系统编码必填', trigger: 'change' }]
      },
      downloadLoading: false,
      isUpdate: false,
      // 角色授权
      roleGrantModel: {
        Id: '',
        Name: '',
        Code: '',
        SystemCode: ''
      },
      roleMenus: [], // 角色菜单,目前只能放到data层
      checkedMenus: [], // 已经选中的角色菜单
      dialogPermissionVisible: false,
      defaultProps: {
        value: 'Id',
        children: 'children',
        label: 'Name'
      }
    }
  },
  created() {
    this.getList()
  },
  methods: {
    optionData(menus) {
      const cloneData = JSON.parse(JSON.stringify(menus)) // 对源数据深度克隆
      return cloneData.filter(father => {
        // 循环所有项，并添加children属性
        const branchArr = cloneData.filter(
          child => father.Id === child.ParentId
        ) // 返回每一项的子级数组
        branchArr.length > 0 ? (father.children = branchArr) : '' // 给父级添加一个children属性，并赋值
        return father.ParentId === 0 // 返回根
      })
    },
    getList() {
      this.listLoading = true
      GetRoles(this.listQuery).then(response => {
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
        Code: ''
      }
    },
    handleCreate() {
      this.resetTemp()
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
      this.isUpdate = false
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    createData() {
      this.$refs['dataForm'].validate(valid => {
        if (valid) {
          AddRole(this.temp).then(() => {
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
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    updateData() {
      this.$refs['dataForm'].validate(valid => {
        if (valid) {
          const tempData = Object.assign({}, this.temp)
          UpdateRole(tempData).then(() => {
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
      this.$notify({
        title: 'Success',
        message: 'Delete Successfully',
        type: 'success',
        duration: 2000
      })
      this.handleFilter()
      // const index = this.list.indexOf(row)
      // this.list.splice(index, 1)
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
    },
    getUserStatus: function(status) {
      switch (status) {
        case 1: return '激活'
        case 2: return '禁用'
        case 3: return '锁定'
        case 4: return '过期'
      }
    },
    getIsAdmin: function(isAdmin) {
      return isAdmin ? '是' : '否'
    },
    resetroleGrantModel() {
      this.roleGrantModel = {
        Id: '',
        Name: '',
        Code: '',
        SystemCode: ''
      }
      this.roleMenus = [] // 角色菜单,目前只能放到data层
      this.checkedMenus = [] // 已经选中的角色菜单
    },
    handleRoleGrant(row) {
      this.resetroleGrantModel()
      const me = this
      this.roleGrantModel = Object.assign({}, row) // copy obj
      // 查询菜单,根据systemcode,查询角色已有菜单,根据roleid
      GetMenus({ 'pageSize': 999, 'pageIndex': 1, 'SystemCode': this.roleGrantModel.SystemCode })
        .then(response => {
          this.roleGrantModel.Menus = me.optionData(response.Data.Content)
          this.roleMenus = this.roleGrantModel.Menus
        })
        // 查询角色已有的菜单
      GetRoleMenus(row.Id).then(response => {
        this.checkedMenus = response.Data.map(r => r.MenuId)
      })
      this.dialogPermissionVisible = true
    },
    GrantRole(roleId) {
      var checkedMenus = this.$refs.tree.getCheckedKeys()
      AddRoleMenus(roleId, checkedMenus).then(() => {
        this.dialogPermissionVisible = false
        this.$notify({
          title: 'Success',
          message: 'GrantRole Successfully',
          type: 'success',
          duration: 2000
        })
        this.handleFilter()
      })
    }
  }
}
</script>
