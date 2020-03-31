<template>
  <div class="app-container">
    <div class="filter-container" style="padding-bottom:10px;">
      <el-input
        v-model="listQuery.Name"
        placeholder="岗位名称"
        style="width: 150px;"
        class="filter-item"
        @keyup.enter.native="handleFilter"
      />
      <el-input
        v-model="listQuery.Code"
        placeholder="岗位编码"
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
      >新增岗位</el-button>
      <el-button
        v-waves
        :loading="downloadLoading"
        class="filter-item"
        type="primary"
        icon="el-icon-download"
        @click="handleDownload"
      >导出岗位</el-button>
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
          <span class="link-type" @click="handleUpdate(row)">{{ row.PositionCode }}</span>
        </template>
      </el-table-column>
      <el-table-column label="上级岗位" align="center">
        <template slot-scope="{row}">
          <span>{{ row.ParentPositionName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="岗位类型" align="center">
        <template slot-scope="{row}">
          <span>{{ getUserPositionType(row.PositionType) }}</span>
        </template>
      </el-table-column>
      <el-table-column label="状态" align="center">
        <template slot-scope="{row}">
          <span>{{ row.PositionStatus==1?"激活":"禁用" }}</span>
        </template>
      </el-table-column>
      <el-table-column label="岗位全称" align="center">
        <template slot-scope="{row}">
          <span>{{ row.FullPositionName }}</span>
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
            <el-form-item label="编码" prop="PositionCode">
              <el-input v-model="temp.PositionCode" />
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
            <el-form-item label="状态" prop="PositionStatus">
              <el-select v-model="temp.PositionStatus" placeholder="请选择">
                <el-option
                  v-for="item in PositionStatusOptions"
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
            <el-form-item label="父级岗位" prop="ParentId">
              <SelectTree
                :props="props"
                :options="optionData"
                :value="valueId"
                :clearable="isClearable"
                :accordion="isAccordion"
                @getValue="getValue($event)"
                @treeSelectCallback="treeSelectCallback($event)"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="岗位类型" prop="PositionType">
              <el-select v-model="temp.PositionType" :disabled="temp.Level!=1" placeholder="请选择">
                <el-option
                  v-for="item in PositionTypeOptions"
                  :key="item.value"
                  :label="item.label"
                  :value="item.value"
                />
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row v-if="dialogStatus==='update'">
          <el-col :span="24">
            <el-form-item label="岗位全称" prop="FullPositionName">
              <el-input v-model="temp.FullPositionName" readonly="readonly" />
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
import { AddPosition, UpdatePosition, GetPositions, DeletePosition, GetPosition } from '@/api/positionmanage'
import waves from '@/directive/waves' // waves directive
import { parseTime } from '@/utils'
// import Pagination from '@/components/Pagination' // secondary package based on el-pagination
import SelectTree from '@/components/TreeSelect'

export default {
  name: 'PositionManage',
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
      PositionStatusOptions: [{ label: '激活', value: 1 }, { label: '禁用', value: 2 }],
      PositionTypeOptions: [
        { label: '指挥军官', value: 1 },
        { label: '技术军官', value: 2 },
        { label: '技师', value: 3 },
        { label: '领班员', value: 4 },
        { label: '值机员', value: 5 },
        { label: '通信员', value: 6 }
      ],
      LevelOptions: [1, 2, 3],
      temp: {
        id: 0,
        name: '',
        PositionCode: '',
        Order: 1,
        ParentId: 0,
        PositionStatus: 0,
        PositionType: 1,
        Level: 1
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
        update: '编辑岗位',
        create: '新增岗位'
      },
      rules: {
        name: [{ required: true, message: '名称必填', trigger: 'change' }],
        PositionCode: [{ required: true, message: '编码必填', trigger: 'change' }],
        ParentId: [{ required: true, message: '父级必填', validator: validateParentId, trigger: 'change' }],
        PositionStatus: [{ required: true, message: '状态必填', trigger: 'change' }]
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
    }, getUserPositionType() {
      return function(position) {
        switch (position) {
          case 1: return '指挥军官'
          case 2: return '技术军官'
          case 3: return '技师'
          case 4: return '领班员'
          case 5: return '值机员'
          case 6: return '通信员'
          default:return '岗位管理'
        }
      }
    }
  },
  created() {
    this.getList()
    this.getPositionForSelectBox()
  },
  methods: {
    getPositionForSelectBox() {
      GetPositions({ 'pageIndex': 1, 'pageSize': 999 }).then(response => {
        this.menuList = response.Data.Content
      })
    },
    getValue(value) {
      this.valueId = value
      this.temp.ParentId = value
    },
    treeSelectCallback(value) {
      if (value === 1) {
        this.temp.Level = 1
      }
      // todo 选择父级岗位的时候,默认填充父级的岗位类别
      GetPosition(value).then(response => {
        this.temp.PositionType = response.Data.PositionType
        this.temp.Level = response.Data.Level + 1
      })
    },
    getList() {
      this.listLoading = true
      GetPositions(this.listQuery).then(response => {
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
        PositionCode: '',
        Order: 1,
        ParentId: 0,
        PositionStatus: 1,
        PositionType: 1,
        Level: 1
      }
    },
    handleCreate() {
      this.getPositionForSelectBox()
      this.resetTemp()
      this.valueId = 1 // 清空给下拉选择框的值
      this.temp.PositionStatus = 1
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
          AddPosition(this.temp).then(response => {
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
      this.getPositionForSelectBox()
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
          UpdatePosition(tempData).then((response) => {
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
      DeletePosition(row.id).then(() => {
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
