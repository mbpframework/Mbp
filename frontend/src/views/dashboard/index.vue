<template>
  <div class="dashboard-container">
    <div class="dashboard-text">name: {{ name }}</div>
    <div class="dashboard-text">role: {{ roles }}</div>
    <div class="dashboard-text">token: {{ token }}</div>
    <div class="dashboard-text">permission_addRoutes: {{ permission_addRoutes }}</div>
    <div class="dashboard-text">avatar: {{ avatar }}</div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'

export default {
  name: 'Dashboard',
  computed: {
    ...mapGetters([
      'name', 'roles', 'token', 'permission_routes', 'avatar', 'permission_addRoutes'
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
