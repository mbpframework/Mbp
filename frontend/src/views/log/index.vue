<template>
  <div class="app-container">
    <div class="filter-container" style="padding-bottom:10px;">
      <el-input
        v-model="listQuery.UserName"
        placeholder="用户名(登录名)"
        style="width: 120px;"
        class="filter-item"
        @keyup.enter.native="handleFilter"
      />
      <el-input
        v-model="listQuery.ClientIP"
        placeholder="客户端Ip"
        style="width: 120px;"
        class="filter-item"
        @keyup.enter.native="handleFilter"
      />
      <el-input
        v-model="listQuery.OpDateTimeBegin"
        placeholder="开始时间"
        style="width: 120px;"
        class="filter-item"
        @keyup.enter.native="handleFilter"
      />
      <el-input
        v-model="listQuery.OpDateTimeEnd"
        placeholder="结束时间"
        style="width: 120px;"
        class="filter-item"
        @keyup.enter.native="handleFilter"
      />
      <el-input
        v-model="listQuery.AppName"
        placeholder="应用名"
        style="width: 120px;"
        class="filter-item"
        @keyup.enter.native="handleFilter"
      />
      <el-input
        v-model="listQuery.ModuleName"
        placeholder="模块名"
        style="width: 120px;"
        class="filter-item"
        @keyup.enter.native="handleFilter"
      />
      <el-input
        v-model="listQuery.OpName"
        placeholder="操作名"
        style="width: 120px;"
        class="filter-item"
        @keyup.enter.native="handleFilter"
      />
      <el-input
        v-model="listQuery.Desc"
        placeholder="描述"
        style="width: 120px;"
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
        prop="Id"
        sortable="custom"
        align="center"
        width="80"
        :class-name="getSortClass('Id')"
      >
        <template slot-scope="{row}">
          <span>{{ row.Id }}</span>
        </template>
      </el-table-column>
      <el-table-column label="用户ID" align="center">
        <template slot-scope="{row}">
          <span>{{ row.UserID }}</span>
        </template>
      </el-table-column>
      <el-table-column label="用户名" align="center">
        <template slot-scope="{row}">
          <span>{{ row.UserName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="角色名" align="center">
        <template slot-scope="{row}">
          <span>{{ row.RoleName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="客户端IP" align="center">
        <template slot-scope="{row}">
          <span>{{ row.ClientIP }}</span>
        </template>
      </el-table-column>
      <el-table-column label="操作时间" align="center">
        <template slot-scope="{row}">
          <span>{{ row.OpDateTime }}</span>
        </template>
      </el-table-column>
      <el-table-column label="应用名" align="center">
        <template slot-scope="{row}">
          <span>{{ row.AppName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="模块名" align="center">
        <template slot-scope="{row}">
          <span>{{ row.ModuleName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="操作名" align="center">
        <template slot-scope="{row}">
          <span>{{ row.OpName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="描述" align="center">
        <template slot-scope="{row}">
          <span>{{ row.Desc }}</span>
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
  </div>
</template>

<script>
import { GetLogs } from '@/api/logmanage'
import waves from '@/directive/waves' // waves directive
import { parseTime } from '@/utils'
import Pagination from '@/components/Pagination' // secondary package based on el-pagination

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
        UserId: undefined,
        ClientIP: undefined,
        OpDateTimeBegin: undefined,
        OpDateTimeEnd: undefined,
        AppName: undefined,
        ModuleName: undefined,
        OpName: undefined,
        Desc: undefined,
        UserName: undefined,
        sort: '+Id'
      },
      sortOptions: [
        { label: 'ID Ascending', key: '+Id' },
        { label: 'ID Descending', key: '-Id' }
      ],
      downloadLoading: false
    }
  },
  created() {
    this.getList()
  },
  methods: {
    getList() {
      this.listLoading = true
      GetLogs(this.listQuery).then(response => {
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
    }
  }
}
</script>
