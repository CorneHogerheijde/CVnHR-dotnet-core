<template>
  <div>
    <input v-model.trim="currentKvkApiSearch.q" :placeholder="$route.query.kvk || 'type anything to search'" @keyup.enter="search" />
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
        if (/^\d{8}$/.test(this.currentKvkApiSearch.q)) { // assuming kvknumber always 8 digit number
          this.$router.push({ query: { kvk: this.currentKvkApiSearch.q } })
        } else {
          this.$router.push({ query: { q: this.currentKvkApiSearch.q } })
        }
      }
    },

    watch: {
      '$route'(to, from) {
        if (!!to.query.q) {
          this.searchKvkApi({ q: to.query.q, startPage: to.query.startPage || 1 });
        } else {
          this.resetKvkApiSearch()
          this.currentKvkApiSearch.q = null;
        }
      }
    },

    created() {
      this.currentKvkApiSearch.q = this.$route.query.q || this.currentKvkApiSearch.q
      this.currentKvkApiSearch.startPage = this.$route.query.startPage || this.currentKvkApiSearch.startPage

      if (!!this.currentKvkApiSearch.q) {
        if (!!this.$route.query.q) {
          this.searchKvkApi({ q: this.$route.query.q, startPage: this.$route.query.startPage || 1 });
        } else {
          const q = this.currentKvkApiSearch.q
          const startPage = this.currentKvkApiSearch.startPage
          this.searchKvkApi({ q, startPage });
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
