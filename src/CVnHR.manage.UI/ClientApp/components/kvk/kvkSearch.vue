<template>
  <div>
    <icon v-if="loading" icon="spinner" pulse />
    <objectTree :item="currentKvkSearch.result"></objectTree>
  </div>
</template>

<script>
  import { mapActions, mapState } from 'vuex'
  import objectTree from '../utilities/objectTree'

  export default {
    components: { objectTree },

    computed: {
      ...mapState({
        currentKvkSearch: state => state.kvkSearch,
        loading: state => state.kvkApiSearch.loading
      })
    },

    methods: {
      ...mapActions(['searchKvk', 'resetKvkSearch']),
      search: function () {
        if (!!this.currentKvkSearch.kvkNumber) {
          console.log(this.currentKvkSearch.kvkNumber)
          this.$router.push({ query: { kvk: this.currentKvkSearch.kvkNumber } })
        }
      }
    },

    watch: {
      '$route'(to, from) {
        if (!!to.query.kvk) {
          this.searchKvk(to.query.kvk);
        } else {
          this.resetKvkSearch()
        }
      }
    },

    created() {
      this.currentKvkSearch.kvkNumber = this.$route.query.kvk || this.currentKvkSearch.kvkNumber
      
      if (!!this.currentKvkSearch.kvkNumber) {
        if (!this.$route.query.kvk) {
          this.search();
        } else {
          this.searchKvk(this.currentKvkSearch.kvkNumber); // TODO, optimize!
        }
      }
    },
  }
</script>

<style scoped>

</style>
