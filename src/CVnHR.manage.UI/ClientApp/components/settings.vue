<template>
  <div>
    <h1>Settings</h1>
    <icon v-if="!currentSettings" icon="spinner" pulse />

    <div v-if="currentSettings && currentSettings.certificates">
      <ul>
        <li v-for="cert in currentSettings.certificates">{{cert}}</li>
      </ul>
    </div>

    <p>{{ currentSettings }}</p>
    <button v-on:click="doSomething">Click me!</button>
  </div> 
</template>

<script>
  import { mapActions, mapState } from 'vuex'

  export default {
    data() {
      return {  };
    },

    computed: {
      ...mapState({
        currentSettings: state => state.settings
      })
    },

    methods: {
      ...mapActions(['updateSettings']),
      doSomething() {
        this.updateSettings({ settings: 'a string of settings' });
      }
    },

    created() {
    }
  }
</script>

<settings>

</settings>
