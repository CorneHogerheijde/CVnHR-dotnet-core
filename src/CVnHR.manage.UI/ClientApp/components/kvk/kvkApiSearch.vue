<template>
  <div>
    <input v-model="currentKvkApiSearch.q" @keyup.enter="search" />
    <button @click="search">search</button>

    <KvkApiSearchResult />

  </div>
</template>

<script>
  import { mapActions, mapState } from 'vuex'
  import KvkApiSearchResult from './kvkApiSearchResult'

  export default {
    components: {
      KvkApiSearchResult
    },

    computed: {
      ...mapState({
        currentKvkApiSearch: state => state.kvkApiSearch
      })
    },

    methods: {
      ...mapActions(['searchKvkApi', 'resetKvkApiSearch']),
      search: function () {
        if (!!this.currentKvkApiSearch.q) {
          this.$router.push({ query: { q: this.currentKvkApiSearch.q } })
        }
      }
    },

    watch: {
      '$route'(to, from) {
        if (!!to.query.q) {
          this.searchKvkApi({ q: to.query.q, startPage: to.query.startPage || 1 });
        } else if (!!to.query.kvk) {
          this.resetKvkApiSearch()
        }
      }
    },

    created() {
      this.currentKvkApiSearch.q = this.$route.query.q || this.currentKvkApiSearch.q
      this.currentKvkApiSearch.startPage = this.$route.query.startPage || this.currentKvkApiSearch.startPage
      
      if (!!this.currentKvkApiSearch.q) {
        if (!!this.$route.query.q) {
          this.search();
        } else {
          const q = this.currentKvkApiSearch.q
          const startPage = this.currentKvkApiSearch.startPage
          this.searchKvkApi({ q, startPage }); // TODO, optimize!
        }
      }
    },
  }
</script>

<style scoped>
  input {
    width: 100%;
  }
</style>
