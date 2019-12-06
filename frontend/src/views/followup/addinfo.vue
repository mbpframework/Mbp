<template>
  <el-dialog title="跟进记录" :visible="dialogVisible" :fullscreen="false" @close="closeDialog">
    <el-form size="small" :inline="true" :model="vales">
      <template>
        <el-form-item label="跟进对象">
          <el-select v-model="vales.followup" placeholder="跟进对象" clearable class="filter-item">
            <el-option
              v-for="item in vales.followupOptions"
              :key="item"
              :label="item"
              :value="item"
            />
          </el-select>
        </el-form-item>
        <el-form-item label="跟进方式">
          <el-select v-model="vales.followuptype" placeholder="跟进方式" clearable class="filter-item">
            <el-option
              v-for="item in vales.followuptypeOptions"
              :key="item"
              :label="item"
              :value="item"
            />
          </el-select>
        </el-form-item>
        <el-form-item label="跟进内容">
          <el-input
            type="textarea"
            :rows="3"
            style="width:400px"
            placeholder="请输入内容"
            v-model="vales.OrderNo"
          ></el-input>
        </el-form-item>

        <el-button type="primary">提交</el-button>
      </template>
      <div>
        <div class="info-box">
          <el-row class="info-title">
            <el-col :span="6">
              <div class="grid-content bg-purple">2019-9-20 18:03:39</div>
            </el-col>
            <el-col :span="6">
              <div class="grid-content bg-purple">王大锤，1335414515</div>
            </el-col>
            <el-col :span="6">
              <div class="grid-content bg-purple">微信</div>
            </el-col>
          </el-row>
          <el-row>
              <el-col :span="24">
              <div class="grid-content bg-purple">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean euismod bibendum laoreet. Proin gravida dolor sit amet lacus accumsan et viverra justo commodo. Proin sodales pulvinar sic tempor. Sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nam fermentum, nulla luctus pharetra vulputate, felis tellus mollis orci, sed rhoncus pronin sapien nunc accuan eget.</div>
            </el-col>
          </el-row>
          </div>
                  <div class="info-box">
          <el-row class="info-title">
            <el-col :span="6">
              <div class="grid-content bg-purple">2019-9-20 18:03:39</div>
            </el-col>
            <el-col :span="6">
              <div class="grid-content bg-purple">王大锤，1335414515</div>
            </el-col>
            <el-col :span="6">
              <div class="grid-content bg-purple">微信</div>
            </el-col>
          </el-row>
          <el-row>
              <el-col :span="24">
              <div class="grid-content bg-purple">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean euismod bibendum laoreet. Proin gravida dolor sit amet lacus accumsan et viverra justo commodo. Proin sodales pulvinar sic tempor. Sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nam fermentum, nulla luctus pharetra vulputate, felis tellus mollis orci, sed rhoncus pronin sapien nunc accuan eget.</div>
            </el-col>
          </el-row>
          </div>
                  <div class="info-box">
          <el-row class="info-title">
            <el-col :span="6">
              <div class="grid-content bg-purple">2019-9-20 18:03:39</div>
            </el-col>
            <el-col :span="6">
              <div class="grid-content bg-purple">王大锤，1335414515</div>
            </el-col>
            <el-col :span="6">
              <div class="grid-content bg-purple">微信</div>
            </el-col>
          </el-row>
          <el-row>
              <el-col :span="24">
              <div class="grid-content bg-purple">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean euismod bibendum laoreet. Proin gravida dolor sit amet lacus accumsan et viverra justo commodo. Proin sodales pulvinar sic tempor. Sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nam fermentum, nulla luctus pharetra vulputate, felis tellus mollis orci, sed rhoncus pronin sapien nunc accuan eget.</div>
            </el-col>
          </el-row>
          </div>
                  <div class="info-box">
          <el-row class="info-title">
            <el-col :span="6">
              <div class="grid-content bg-purple">2019-9-20 18:03:39</div>
            </el-col>
            <el-col :span="6">
              <div class="grid-content bg-purple">王大锤，1335414515</div>
            </el-col>
            <el-col :span="6">
              <div class="grid-content bg-purple">微信</div>
            </el-col>
          </el-row>
          <el-row>
              <el-col :span="24">
              <div class="grid-content bg-purple">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean euismod bibendum laoreet. Proin gravida dolor sit amet lacus accumsan et viverra justo commodo. Proin sodales pulvinar sic tempor. Sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nam fermentum, nulla luctus pharetra vulputate, felis tellus mollis orci, sed rhoncus pronin sapien nunc accuan eget.</div>
            </el-col>
          </el-row>
          </div>
          <pagination v-show="vales.total>vales.limit" :total="vales.total" :page.sync="vales.page" :limit.sync="vales.limit" @pagination="getList" />
      </div>
    </el-form>
  </el-dialog>
</template>
<script>
import pagination from '@/components/Pagination'
export default {
    components: { pagination},
  props: {
    isvisbleadd: Boolean,
    vales: Object
  },
  /*data() {
    return {
      followup: undefined,
      followuptype: undefined,
      followupOptions,
      followuptypeOptions
    };
  },*/
  created() {
    this.getList()
    this.getfollowupOptions();
    this.getfollowuptypeOptions();
  },
  computed: {
    dialogVisible: function() {
      return this.isvisbleadd;
    }
  },
  methods: {
    getfollowupOptions() {
      this.vales.followupOptions = ["张三", "李四"];
      //this.listLoading = true;
      //fetchList(this.listQuery).then(response => {

      //});
    },
    getList() {
      this.listLoading = true
      this.vales.total=11
    },
    getfollowuptypeOptions() {
      this.vales.followuptypeOptions = ["电话", "微信"];
    },
    closeDialog() {
      this.$emit("on-close");
    }
  }
};
</script>
<style scoped>
.info-box {
  padding: 1rem;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.12), 0 0 6px rgba(0, 0, 0, 0.04);
  margin-bottom: 1rem;
}
.info-title{
    color:#999
}
</style>