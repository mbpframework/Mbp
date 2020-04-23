<template>
  <div class="dashboard-container">
    <!-- <div class="dashboard-text">name: {{ name }}</div>
    <div class="dashboard-text">role: {{ roles }}</div> -->
    <h1>欢迎使用表报系统</h1>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'

export default {
  name: 'Dashboard',
  computed: {
    ...mapGetters([
      'name', 'roles'
    ])
  }
}
</script>

<style lang="scss" scoped>
.dashboard {
  &-container {
    margin: 30px;
  }
  &-text {
    font-size: 30px;
    line-height: 46px;
  }
}
</style>
