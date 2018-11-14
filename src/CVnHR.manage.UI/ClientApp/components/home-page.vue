<template>
  <div>
    <h1>Zoek bij KvK</h1>

    <kvkApiSearch></kvkApiSearch>

  </div>
</template>

<script>
  import { mapActions, mapState } from 'vuex'
  import kvkApiSearch from './kvk/kvkApiSearch'

  export default {
    components: {
      kvkApiSearch
    }
  }
</script>

<style>
</style>
