<template>
  <div>
    <input v-model="currentKvkApiSearch.q" @keyup.enter="search" />
    <button @click="search">search</button>
  </div>
</template>

<script>
  import { mapActions, mapState } from 'vuex'

  export default {
    computed: {
      ...mapState({
        currentKvkApiSearch: state => state.kvkApiSearch
      })
    },

    methods: {
      ...mapActions(['searchKvkApi']),
      search: function () {
        this.$router.push({ query: { q: this.currentKvkApiSearch.q }})
      }
    },

    watch: {
      '$route'(to, from) {
        if (to !== from) {
          this.searchKvkApi(to.query.q);
        }
      }
    },

    created() {
      this.currentKvkApiSearch.q = this.$route.query.q
      if (this.currentKvkApiSearch.q) {
        this.searchKvkApi(this.currentKvkApiSearch.q);
      }
    },
  }
</script>

<style scoped>
  input {
    width: 100%;
  }
</style>
