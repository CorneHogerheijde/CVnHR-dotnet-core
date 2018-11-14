import axios from 'axios'

const updateSettingsType = 'updateSettingsType'
const ensureSettingsType = 'ensureSettings'

const settingsActions = {
  ensureSettings: async ({ commit }) => {
    commit(ensureSettingsType)
  },
  updateSettings: ({ commit }, settings) => {
    commit(updateSettingsType, settings)
  }
}

const settingsMutations = {
  async [ensureSettingsType] (state) {
    let settings = null
    try {
      let response = await axios.get(`/api/settings/GetSettings`)
      settings = response.data
    } catch (err) {
      window.alert(err)
      console.log(err)
    }
    state.settings = settings
  },
  async [updateSettingsType](state, settings) {
    console.log('updating settings!', settings)
    try {
      axios.put(`/api/settings/UpdateKvkApiSettings`, settings.kvkApiSettings)
    } catch (err) {
      window.alert(err)
      console.log(err)
    }
    state.settings = settings
  }
}

export { settingsActions, settingsMutations }
