<template>
  <div class="dashboard-editor-container">
    <el-row :gutter="100">
      <el-col :span="12">
        <div class="grid-content bg-purple">
          <el-table :data="listTrainHourRank">
            <el-table-column width="50" label="排名">
              <template slot-scope="{row}">
                {{ row.Rank }}
              </template>
            </el-table-column>
            <el-table-column label="姓名" align="center">
              <template slot-scope="{row}">
                {{ row.UserName }}
              </template>
            </el-table-column>
            <el-table-column label="单位" align="center">
              <template slot-scope="{row}">
                {{ row.DeptName }}
              </template>
            </el-table-column>
            <el-table-column label="训练时间(小时)" align="center">
              <template slot-scope="{row}">
                {{ row.TrainHour }}
              </template>
            </el-table-column>
          </el-table>
        </div>
      </el-col>
      <el-col :span="12">
        <div class="grid-content bg-purple">
          <el-table :data="listTrainScoreRank">
            <el-table-column width="50" label="排名">
              <template slot-scope="{row}">
                {{ row.Rank }}
              </template>
            </el-table-column>
            <el-table-column label="姓名" align="center">
              <template slot-scope="{row}">
                {{ row.UserName }}
              </template>
            </el-table-column>
            <el-table-column label="单位" align="center">
              <template slot-scope="{row}">
                {{ row.DeptName }}
              </template>
            </el-table-column>
            <el-table-column label="训练成绩(分)" align="center">
              <template slot-scope="{row}">
                {{ row.TrainScore }}
              </template>
            </el-table-column>
          </el-table>
        </div>
      </el-col>
    </el-row>
  </div>
</template>

<script>

export default {
  filters: {
    statusFilter(status) {
      const statusMap = {
        success: 'success',
        pending: 'danger'
      }
      return statusMap[status]
    },
    orderNoFilter(str) {
      return str.substring(0, 30)
    }
  },
  data() {
    return {
      listTrainHourRank: [],
      listTrainScoreRank: []
    }
  },
  created() {
    this.fetchData()
  },
  methods: {
    fetchData() {
      this.listTrainHourRank = [
        { Rank: 1, UserName: '杜斌', DeptName: '一营', TrainHour: 432.34 },
        { Rank: 2, UserName: '李洋', DeptName: '二营', TrainHour: 386.45 },
        { Rank: 3, UserName: '杨冲', DeptName: '一营', TrainHour: 205.36 },
        { Rank: 4, UserName: '梅辰东', DeptName: '三营', TrainHour: 198.53 },
        { Rank: 5, UserName: '苏焱', DeptName: '四营', TrainHour: 186.47 },
        { Rank: 6, UserName: '韩彬', DeptName: '一营', TrainHour: 177.63 },
        { Rank: 7, UserName: '尹化松', DeptName: '三营', TrainHour: 157.25 },
        { Rank: 8, UserName: '张荣刚', DeptName: '四营', TrainHour: 154.38 },
        { Rank: 9, UserName: '王挺', DeptName: '二营', TrainHour: 142.14 },
        { Rank: 10, UserName: '郭顺', DeptName: '三营', TrainHour: 139.46 }]
      this.listTrainScoreRank = [
        { Rank: 1, UserName: '杜斌', DeptName: '一营', TrainScore: 95 },
        { Rank: 2, UserName: '李洋', DeptName: '二营', TrainScore: 86 },
        { Rank: 3, UserName: '杨冲', DeptName: '一营', TrainScore: 84 },
        { Rank: 4, UserName: '梅辰东', DeptName: '三营', TrainScore: 81 },
        { Rank: 5, UserName: '苏焱', DeptName: '四营', TrainScore: 76 },
        { Rank: 6, UserName: '韩彬', DeptName: '一营', TrainScore: 73 },
        { Rank: 7, UserName: '尹化松', DeptName: '三营', TrainScore: 71 },
        { Rank: 8, UserName: '张荣刚', DeptName: '四营', TrainScore: 69 },
        { Rank: 9, UserName: '王挺', DeptName: '二营', TrainScore: 68 },
        { Rank: 10, UserName: '郭顺', DeptName: '三营', TrainScore: 51 }]
    }
  }
}
</script>

<style lang="scss" scoped>
.dashboard-editor-container {
  padding: 32px;
  background-color: rgb(240, 242, 245);
  position: relative;

  .github-corner {
    position: absolute;
    top: 0px;
    border: 0;
    right: 0;
  }

  .chart-wrapper {
    background: #fff;
    padding: 16px 16px 0;
    margin-bottom: 32px;
  }
}

@media (max-width:1024px) {
  .chart-wrapper {
    padding: 8px;
  }
}
</style>
