<template>
  <div class="OrderDetail-container">
    <div class="info-box">
      <div class="info-title">订单信息</div>
      <div>
        <el-row :gutter="20">
          <el-col :span="6">
            <div class="grid-content bg-purple">订单号：{{postForm.OrderNo}}</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">渠道：{{postForm.Channel}}</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">订单金额：{{postForm.OrderAmount}}</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">快递公司：{{postForm.CSC}}</div>
          </el-col>

          <el-col :span="6">
            <div class="grid-content bg-purple">下单时间：{{postForm.OrderTime}}</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">物流编号：{{postForm.LogisticsNo}}</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">订单状态：{{postForm.OrderStatus}}</div>
          </el-col>
        </el-row>
      </div>
    </div>
    <div class="info-box">
      <div class="info-title">商品信息</div>
      <div v-for="item in postForm.GoodsInfos" :key="item.id">
        <el-row>
          <el-col :span="24">
            <div class="grid-content bg-purple">商品名称：{{item.GoodsName}}</div>
          </el-col>
        </el-row>
        <el-row :gutter="20">
          <el-col :span="6">
            <div class="grid-content bg-purple">商品类型：{{item.GoodsCategory}}</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">商品数量：{{item.GoodsAmount}}</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">商品规格：{{item.GoodsSpecs}}</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">单价：{{item.GoodsPrice}}</div>
          </el-col>
        </el-row>
        <template v-if="item.GoodsCategory==='酒店'">
        <el-row :gutter="20">
          <el-col :span="6">
            <div class="grid-content bg-purple">酒店入住时间：{{item.HotelInTime}}</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">酒店离店时间：{{item.HotelLeaveTime}}</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">酒店目的地：{{item.HotelDestination}}</div>
          </el-col>
        </el-row>
        </template>
        <template  v-else-if="item.GoodsCategory==='票务'">
        <el-row :gutter="20">
          <el-col :span="6">
            <div class="grid-content bg-purple">票务出行时间：{{item.TicketStartTime}}</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">票务目的地：{{item.TicketDestination}}</div>
          </el-col>
        </el-row>
        </template>
        <template  v-else-if="item.GoodsCategory==='用车'">
        <el-row :gutter="20">
          <el-col :span="6">
            <div class="grid-content bg-purple">用车出行时间：{{item.CarStartTime}}</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">用车目的地：{{item.CarDestination}}</div>
          </el-col>
        </el-row>
        </template>
        <template  v-else-if="item.GoodsCategory==='机票'">
        <el-row :gutter="20">
          <el-col :span="6">
            <div class="grid-content bg-purple">机票类型：{{item.PlaneTicketType}}</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">机场起点：{{item.PlaneTicketOrigin}}</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">机场止点：{{item.PlaneTicketDestination}}</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">时间起点：{{item.PlaneStartTime}}</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">时间止点：{{item.PlaneEndTime}}</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">航司：{{item.AirlineCompany}}</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">航班号：{{item.FlightNumber}}</div>
          </el-col>
        </el-row>
        </template>
        <template  v-else-if="item.GoodsCategory==='日游'">
        <el-row :gutter="20">
          <el-col :span="6">
            <div class="grid-content bg-purple">日游出行日期：{{item.DayTripStartTime}}</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">日游目的地：{{item.DayTripDestination}}</div>
          </el-col>
        </el-row>
        </template>
      </div>
    </div>
    <div class="info-box">
      <div class="info-title">出行信息</div>
      <div>
        <el-row :gutter="20">
          <el-col :span="6">
            <div class="grid-content bg-purple">出行时间:</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">结束时间：</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">目的地：</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple"></div>
          </el-col>
        </el-row>
        <el-row :gutter="20">
          <el-col :span="24">
            <div class="grid-content bg-purple">出行人1</div>
          </el-col>
        </el-row>
        <el-row :gutter="20">
          <el-col :span="6">
            <div class="grid-content bg-purple">姓名:</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">手机</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">出行天数</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">生日</div>
          </el-col>
        </el-row>
        <el-row :gutter="20">
          <el-col :span="24">
            <div class="grid-content bg-purple">出行人2</div>
          </el-col>
        </el-row>
        <el-row :gutter="20">
          <el-col :span="6">
            <div class="grid-content bg-purple">姓名:</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">手机</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">出行天数</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">生日</div>
          </el-col>
        </el-row>
        <el-row :gutter="20">
          <el-col :span="24">
            <div class="grid-content bg-purple">联系人:</div>
          </el-col>
        </el-row>
        <el-row :gutter="20">
          <el-col :span="6">
            <div class="grid-content bg-purple">姓名:</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">手机</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">微信</div>
          </el-col>
          <el-col :span="6">
            <div class="grid-content bg-purple">生日</div>
          </el-col>
        </el-row>
      </div>
    </div>
    <div>
      <el-button type="primary" @click="addbtn">跟进</el-button>
      <add-info :vales="showdata" :isvisbleadd="addinfoshow"  @on-close="closeDialog"></add-info>
    </div> 
  </div>
</template>
<script>
import { fetchList, fetchArticle } from "@/api/order";
const defaultForm = {
  id: undefined,
  OrderNo: undefined, //订单号
  Channel: "", // 渠道
  OrderAmount: "", // 订单金额
  CSC: "", // 快递公司
  OrderTime: "", // 下单时间
  LogisticsNo: "", // 物流编号
  OrderStatus: "",
  OrdererUser: {
    OrderInfoId: "", //订单外键
    UserName: "",
    Phone: "",
    Wechat: "",
    BirthDay: "",
    Sex: "",
    Email: ""
  }, // 下单人
  OrderConsumers: [
    {
      OrderInfoId: "",
      UserName: "",
      Phone: "",
      Wechat: "",
      Birthday: "",
      Sex: "",
      Email: ""
    }
  ], //出行人/收货人
  OrderContacts: {
    OrderInfoId: "",
    UserName: "",
    Phone: ""
  }, //联系人
  Payment: {
    OrderInfoId: "",
    PaymentType: "",
    PayTime: "",
    PayStatus: ""
  }, //支付信息
  ShippingAddress: {
    OrderInfoId: "",
    MainInfo: "",
    Detail: "",
    PostCode: ""
  },
  GoodsInfos: [
    {
      OrderInfoId: "",
      GoodsCategory: "",
      GoodsId: "",
      GoodsAmount: "",
      GoodsName: "",
      GoodsSpecs: "",
      GoodsPrice: "",
      GoodsType: "",
      HotelInTime: "",
      HotelLeaveTime: "",
      HotelDestination: "",
      TicketStartTime: "",
      TicketDestination: "",
      CarStartTime: "",
      CarDestination: "",
      PlaneTicketType: "",
      PlaneTicketOrigin: "",
      PlaneTicketDestination: "",
      PlaneStartTime: "",
      PlaneEndTime: "",
      AirlineCompany: "",
      FlightNumber: "",
      DayTripStartTime: "",
      DayTripDestination: ""
    }
  ]
};

export default {
  name: "OrderDetail",
  components:{
    'add-info': ()=>import('../followup/addinfo')
  },
  data() {
    return {
      postForm: Object.assign({}, defaultForm),
      loading: false,
      userListOptions: [],
      tempRoute: {},
      addinfoshow:false,
      showdata:{
        OrderNo:'',
        page: 1,
        limit: 10
      }
    };
  },
  computed: {},
  created() {
    const id = this.$route.params && this.$route.params.id;
    this.postForm.id = id
    this.showdata.OrderNo=id
    this.postForm.OrderNo = id
    this.tempRoute = Object.assign({}, this.$route);
    this.setTagsViewTitle();
    this.setPageTitle();
    this.fetchData(id);
  },
  methods: {
    addbtn(){
      this.addinfoshow=true
    },
    closeDialog(){
      this.addinfoshow=false
    },
    fetchData(id) {
      fetchArticle(id)
        .then(response => {
          this.postForm = response.data;

          // just for test
          this.postForm.title += `   Article Id:${this.postForm.id}`;
          this.postForm.content_short += `   Article Id:${this.postForm.id}`;

          // set tagsview title
        })
        .catch(err => {
          console.log(err);
        });
    },
    setTagsViewTitle() {
      const title = "订单详情";
      const route = Object.assign({}, this.tempRoute, {
        title: `${title}-${this.postForm.id}`
      });
      console.log(route);
      this.$store.dispatch("tagsView/updateVisitedView", route);
    },
    setPageTitle() {
      const title = "订单详情";
      document.title = `${title} - ${this.postForm.id}`;
    },
    submitForm() {
      console.log(this.postForm);
      this.$refs.postForm.validate(valid => {
        if (valid) {
          this.loading = true;
          this.$notify({
            title: "成功",
            message: "发布文章成功",
            type: "success",
            duration: 2000
          });
          this.postForm.status = "published";
          this.loading = false;
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    }
  }
};
</script>
<style scoped>
.OrderDetail-container {
  padding: 20px;
  color: #999;
  line-height: 2rem;
}
.info-box {
  padding: 1rem;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.12), 0 0 6px rgba(0, 0, 0, 0.04);
  margin-bottom: 1rem;
}
.info-title {
  border-bottom: 1px solid rgb(235, 235, 235);
  color: #666;
  font-weight: bold;
}
</style>