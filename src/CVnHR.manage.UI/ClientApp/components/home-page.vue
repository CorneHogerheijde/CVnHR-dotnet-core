<template>
  <div>
    <h1>Zoek bij KvK</h1>

    <kvkApiSearch></kvkApiSearch>
    <kvkSearch></kvkSearch>
  </div>
</template>

<script>
  import { mapActions, mapState } from 'vuex'
  import kvkApiSearch from './kvk/kvkApiSearch'
  import kvkSearch from './kvk/kvkSearch'

  export default {
    components: {
      kvkApiSearch,
      kvkSearch
    }
  }
</script>

<style>
</style>
