<template>
    <div id="app" class="container-fluid">
        <div class="row">
            <div class="col-md-2 leftcol">
                <nav-menu params="route: route"></nav-menu>
            </div>
            <div class="col-md-10">
                <router-view></router-view>
            </div>
        </div>
    </div>
</template>

<script>
  import NavMenu from './nav-menu'
  import { mapActions, mapState } from 'vuex'

    export default {
      components: {
        'nav-menu': NavMenu
      },

      data () {
        return {}
      },

      methods: {
        ...mapActions(['ensureSettings']),
      },

      created() {
        this.ensureSettings();
      }
    }
</script>

<style>
</style>
