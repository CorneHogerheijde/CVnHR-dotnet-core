<template>
  <div>
    <input v-model="currentKvkApiSearch.q" @keyup.enter="search" />
    <button @click="search">search</button>
    <icon v-if="currentKvkApiSearch.loading" icon="spinner" pulse />

    <div v-if="currentKvkApiSearch.result">
      <p>{{currentKvkApiSearch.result.totalItems}} resultaten</p>
      <strong> TODO: show results in table component and implement paging </strong>

      <div>{{currentKvkApiSearch.result}}</div>

    </div>
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
          this.searchKvkApi(to.query.q, to.query.startPage || 1);
        }
      }
    },

    created() {
      this.currentKvkApiSearch.q = this.$route.query.q || this.currentKvkApiSearch.q
      this.currentKvkApiSearch.startPage = this.$route.query.startPage || this.currentKvkApiSearch.startPage
      if (this.currentKvkApiSearch.q) {
        if (!this.$route.query.q) {
          this.search();
        } else {
          this.searchKvkApi(this.currentKvkApiSearch.q, this.currentKvkApiSearch.startPage); // TODO, optimize!
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
