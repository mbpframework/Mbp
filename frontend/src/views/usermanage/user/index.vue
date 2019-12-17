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
        v-model="listQuery.LoginName"
        placeholder="登录名"
        style="width: 150px;"
        class="filter-item"
        @keyup.enter.native="handleFilter"
      />
      <el-select
        v-model="listQuery.isAdmin"
        placeholder="是否超管"
        style="width: 110px"
        class="filter-item"
        @change="handleFilter"
      >
        <el-option
          v-for="item in isAdminOptions"
          :key="item.key"
          :label="item.label"
          :value="item.key"
        />
      </el-select>
      <!-- <el-select
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
      </el-select> -->
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
      >新增用户</el-button>
      <el-button
        v-waves
        :loading="downloadLoading"
        class="filter-item"
        type="primary"
        icon="el-icon-download"
        @click="handleDownload"
      >导出用户</el-button>
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
      <el-table-column label="登录名" align="center">
        <template slot-scope="{row}">
          <span>{{ row.LoginName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="姓名" align="center">
        <template slot-scope="{row}">
          <span class="link-type" @click="handleUpdate(row)">{{ row.UserName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="用户编码" align="center">
        <template slot-scope="{row}">
          <span>{{ row.Code }}</span>
        </template>
      </el-table-column>
      <el-table-column label="电子邮箱" align="center">
        <template slot-scope="{row}">
          <span>{{ row.Email }}</span>
        </template>
      </el-table-column>
      <el-table-column label="电话号码" align="center">
        <template slot-scope="{row}">
          <span>{{ row.PhoneNumber }}</span>
        </template>
      </el-table-column>
      <el-table-column label="状态" align="center">
        <template slot-scope="{row}">
          <span>{{ getUserStatus(row.UserStatus) }}</span>
        </template>
      </el-table-column>
      <el-table-column label="是否超管" align="center">
        <template slot-scope="{row}">
          <span>{{ getIsAdmin(row.IsAdmin) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="操作"
        align="center"
        width="350"
        class-name="small-padding fixed-width"
      >
        <template slot-scope="{row}">
          <el-button type="primary" size="mini" @click="handleUpdate(row)">编辑</el-button>
          <el-button type="primary" size="mini" @click="handleGrant(row)">授权</el-button>
          <el-button type="warning" size="mini" @click="handlePwdReset(row)">修改密码</el-button>
          <el-button
            v-if="row.status!='deleted'&&row.LoginName!='admin'"
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
        label-width="70px"
        style="width: 400px; margin-left:50px;"
      >
        <el-row>
          <el-col :span="12">
            <el-form-item v-show="false" label="ID" prop="Id">
              <el-input v-model="temp.Id" />
            </el-form-item>
            <el-form-item label="登录名" prop="LoginName">
              <el-input v-model="temp.LoginName" :readonly="dialogStatus==='update'?'readonly':false" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="姓名" prop="UserName">
              <el-input v-model="temp.UserName" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="用户编码" prop="Code">
              <el-input v-model="temp.Code" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="手机号码" prop="PhoneNumber">
              <el-input v-model="temp.PhoneNumber" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="是否超管">
              <el-switch v-model="temp.IsAdmin" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="电子邮箱" prop="Email">
              <el-input v-model="temp.Email" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">取消</el-button>
        <el-button type="primary" @click="dialogStatus==='create'?createData():updateData()">确认</el-button>
      </div>
    </el-dialog>

    <el-dialog title="用户授权" :visible.sync="dialogGrantFormVisible">
      <el-form
        ref="dataForm"
        label-position="right"
        label-width="70px"
        style="width: 400px; margin-left:50px;"
      >
        <el-row>
          <el-col :span="24">数据建模平台</el-col>
          <el-col :span="24">
            <el-checkbox v-model="grantTemp.checkMdpAll" :indeterminate="grantTemp.isMdpIndeterminate" @change="handleCheckMdpAllChange">全选</el-checkbox>
            <div style="margin: 15px 0;" />
            <el-checkbox-group v-model="grantTemp.checkedMdpRoles" @change="handleCheckedMdpRolesChange">
              <el-checkbox v-for="role in grantTemp.userMdpRoles" :key="role.Id" :label="role.Id" border>{{ role.Name }}</el-checkbox>
            </el-checkbox-group></el-col>
        </el-row>
        <el-row style="margin-top:50px;">
          <el-col :span="24">大数据平台</el-col>
          <el-col :span="24">
            <el-checkbox v-model="grantTemp.checkMbdpAll" :indeterminate="grantTemp.isMbpdIndeterminate" @change="handleCheckMbdpAllChange">全选</el-checkbox>
            <div style="margin: 15px 0;" />
            <el-checkbox-group v-model="grantTemp.checkedMbdpRoles" @change="handleCheckedMbdpRolesChange">
              <el-checkbox v-for="role in grantTemp.userMbdpRoles" :key="role.Id" :label="role.Id" border>{{ role.Name }}</el-checkbox>
            </el-checkbox-group></el-col>
        </el-row>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogGrantFormVisible = false">取消</el-button>
        <el-button type="primary" @click="GrantUserRole()">确认</el-button>
      </div>
    </el-dialog>
    <el-dialog title="密码重置" :visible.sync="dialogPwdResetFormVisible">
      <el-form
        ref="dataForm"
        :rules="pwdResetRules"
        :model="pwdTemp"
        label-position="right"
        label-width="70px"
        style="width: 400px; margin-left:50px;"
      >
        <el-row>
          <el-col>
            <el-form-item label="登录名" prop="LoginName">
              <el-input v-model="pwdTemp.LoginName" />
            </el-form-item>
          </el-col>
          <el-col>
            <el-form-item label="密码" prop="Password">
              <el-input v-model="pwdTemp.Password" show-password />
            </el-form-item>
          </el-col>
        </el-row>

      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogPwdResetFormVisible = false">取消</el-button>
        <el-button type="primary" @click="PwdReset()">确认</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import { AddUser, UpdateUser, GetUser, DeleteUser, GetUserRoles, AddUserRoles, RestPwd } from '@/api/usermanage'
import { GetRoles } from '@/api/rolemanage'
import waves from '@/directive/waves' // waves directive
import { parseTime } from '@/utils'
import Pagination from '@/components/Pagination' // secondary package based on el-pagination
import crypto from 'crypto'

export default {
  name: 'UserManage',
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
        isAdmin: undefined,
        UserName: undefined,
        LoginName: undefined,
        sort: '+Id'
      },
      isAdminOptions: [
        { label: '全部', key: '' },
        { label: '是', key: 'Y' },
        { label: '否', key: 'N' }
      ],
      sortOptions: [
        { label: 'ID Ascending', key: '+Id' },
        { label: 'ID Descending', key: '-Id' }
      ],
      temp: {
        Id: 0,
        LoginName: '',
        UserName: '',
        Code: '',
        Email: '',
        PhoneNumber: '',
        IsAdmin: false,
        UserStatus: 1
      },
      dialogFormVisible: false,
      dialogGrantFormVisible: false,
      dialogPwdResetFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑用户',
        create: '新增用户'
      },
      rules: {
        LoginName: [
          { required: true, message: '登录名必填', trigger: 'change' }
        ],
        Password: [{ required: true, message: '密码必填', trigger: 'change' }],
        UserName: [{ required: true, message: '姓名必填', trigger: 'change' }]
      },
      pwdResetRules: {
        Password: [{ required: true, message: '密码必填', trigger: 'change' }]
      },
      downloadLoading: false,
      isUpdate: false,
      grantTemp: {
        currentUserId: 0,
        checkMdpAll: false,
        checkMbdpAll: false,
        checkedMdpRoles: [],
        checkedMbdpRoles: [],
        userMdpRoles: [],
        userMbdpRoles: [],
        isMdpIndeterminate: true,
        isMbpdIndeterminate: true
      },
      pwdTemp: {
        LoginName: '',
        Password: ''
      }
    }
  },
  created() {
    this.getList()
  },
  methods: {
    getList() {
      this.listLoading = true
      GetUser(this.listQuery).then(response => {
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
        LoginName: '',
        UserName: '',
        Code: '',
        Email: '',
        PhoneNumber: '',
        IsAdmin: false,
        UserStatus: 1
      }
    },
    restGrantTemp() {
      this.grantTemp = {
        currentUserId: 0,
        checkMdpAll: false,
        checkMbdpAll: false,
        checkedMdpRoles: [],
        checkedMbdpRoles: [],
        userMdpRoles: [],
        userMbdpRoles: [],
        isMdpIndeterminate: true,
        isMbpdIndeterminate: true
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
          var md5 = crypto.createHash('md5')
          md5.update(this.temp.Password)
          this.temp.Password = md5.digest('hex')
          AddUser(this.temp).then(() => {
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
          UpdateUser(tempData).then(() => {
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
      DeleteUser(row.Id).then(() => {
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
    handlePwdReset(row) {
      this.pwdTemp = Object.assign({}, row)

      this.dialogPwdResetFormVisible = true
    },
    PwdReset() {
      this.$refs['dataForm'].validate(valid => {
        if (valid) {
          const tempData = Object.assign({}, this.pwdTemp)
          var md5 = crypto.createHash('md5')
          md5.update(tempData.Password)
          tempData.Password = md5.digest('hex')
          RestPwd(tempData.LoginName, tempData.Password).then(() => {
            this.dialogPwdResetFormVisible = false
            this.$notify({
              title: 'Success',
              message: 'ReSetPwd Successfully',
              type: 'success',
              duration: 2000
            })
            this.handleFilter()
          })
        }
      })
    },
    handleGrant(row) {
      this.restGrantTemp()
      const that = this
      this.grantTemp.currentUserId = row.Id
      // 获取所有角色
      GetRoles({ pageIndex: 1, pageSize: 999, SystemCode: 'mdp' }).then(response => {
        this.grantTemp.userMdpRoles = response.Data.Content
      })
      GetRoles({ pageIndex: 1, pageSize: 999, SystemCode: 'mbdp' }).then(response => {
        this.grantTemp.userMbdpRoles = response.Data.Content
      })
      // 获取选择中角色
      GetUserRoles(row.Id, 'mdp').then((response) => {
        that.grantTemp.checkedMdpRoles = response.Data.map(r => r.RoleId)
      })
      GetUserRoles(row.Id, 'mbdp').then((response) => {
        this.grantTemp.checkedMbdpRoles = response.Data.map(r => r.RoleId)
      })

      this.dialogGrantFormVisible = true
    },
    GrantUserRole() {
      const roleIds = this.grantTemp.checkedMdpRoles.concat(this.grantTemp.checkedMbdpRoles)
      const userId = this.grantTemp.currentUserId
      AddUserRoles(userId, roleIds).then(() => {
        this.dialogGrantFormVisible = false
        this.$notify({
          title: 'Success',
          message: 'Grant Successfully',
          type: 'success',
          duration: 2000
        })
        this.handleFilter()
      })
    },
    handleCheckMdpAllChange(val) {
      this.grantTemp.checkedMdpRoles = val ? this.grantTemp.userMdpRoles.map(r => r.Id) : []
      this.grantTemp.isMdpIndeterminate = false
    },
    handleCheckedMdpRolesChange(value) {
      const checkedMdbCount = value.length
      this.grantTemp.checkMdpAll = checkedMdbCount === this.grantTemp.userMdpRoles.length
      this.grantTemp.isMdpIndeterminate = checkedMdbCount > 0 && checkedMdbCount < this.grantTemp.userMdpRoles.length
    },
    handleCheckMbdpAllChange(val) {
      this.grantTemp.checkedMbdpRoles = val ? this.grantTemp.userMbdpRoles.map(r => r.Id) : []
      this.grantTemp.isMbpdIndeterminate = false
    },
    handleCheckedMbdpRolesChange(value) {
      const checkedMbdpCount = value.length
      this.grantTemp.checkMbdpAll = checkedMbdpCount === this.grantTemp.userMbdpRoles.length
      this.grantTemp.isMbpdIndeterminate = checkedMbdpCount > 0 && checkedMbdpCount < this.grantTemp.userMbdpRoles.length
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
    }
  }
}
</script>
