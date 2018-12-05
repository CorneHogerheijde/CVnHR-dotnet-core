<template>
  <div>
    <h1>Settings</h1>

    <icon v-if="!currentSettings" icon="spinner" pulse />

    <div v-if="currentSettings && currentSettings.certificates">
      <ul>
        <li v-for="cert in currentSettings.certificates">{{cert}}</li>
      </ul>

      <label>
        KvK API key
        <input v-model="currentSettings.kvkApiSettings.apiKey" />
      </label>
      <label>
        Kvk base url
        <input v-model="currentSettings.kvkApiSettings.baseUrl" />
      </label>
      <label>
        Kvk search url
        <input v-model="currentSettings.kvkApiSettings.searchUrl" />
      </label>
      <label>
        Kvk profile url
        <input v-model="currentSettings.kvkApiSettings.profileUrl" />
      </label>
      <button @click="updateSettings(currentSettings)">update settings</button>
    </div>
  </div> 
</template>

<script>
  import { mapActions, mapState } from 'vuex'

  export default {
    computed: {
      ...mapState({
        currentSettings: state => state.settings
      })
    },

    methods: {
      ...mapActions(['updateSettings'])
    }
  }
</script>

<style>
  label, input {
    width: 100%;
  }
</style>
