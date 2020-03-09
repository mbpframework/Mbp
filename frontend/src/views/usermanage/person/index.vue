<template>
  <div>
    <el-container>
      <el-aside>
        <el-menu :default-openeds="['1', '3']" style="border:none">
          <el-submenu index="1">
            <template slot="title"><i class="el-icon-message" />导航一</template>
            <el-menu-item-group>
              <template slot="title">分组一</template>
              <el-menu-item index="1-1">选项1</el-menu-item>
              <el-menu-item index="1-2">选项2</el-menu-item>
            </el-menu-item-group>
            <el-menu-item-group title="分组2">
              <el-menu-item index="1-3">选项3</el-menu-item>
            </el-menu-item-group>
            <el-submenu index="1-4">
              <template slot="title">选项4</template>
              <el-menu-item index="1-4-1">选项4-1</el-menu-item>
            </el-submenu>
          </el-submenu>
        </el-menu>
      </el-aside>
      <el-main>
        <div style="background-color:green;">2222</div>
      </el-main>
    </el-container>
  </div>
</template>
<style scoped>
.el-aside {
    width:auto;
display: block;
position:absolute;
top:0;
bottom: 0;
overflow-y:scroll;
}
</style>
