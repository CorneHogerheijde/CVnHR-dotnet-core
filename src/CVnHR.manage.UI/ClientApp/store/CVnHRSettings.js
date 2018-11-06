import axios from 'axios'

const updateSettingsType = 'updateSettingsType'

const settingsActions = {
  ensureSettings: async ({ commit }) => {
    var settings = null
    try {
      let response = await axios.get(`/api/settings/GetSettings`)
      console.log(response)
      settings = response.data
    } catch (err) {
      window.alert(err)
      console.log(err)
    }

    commit(updateSettingsType, settings)
  },
  updateSettings: ({ commit }, settings) => {
    console.log('updating settings!', settings)
    settings = 'settings updated.'
    commit(updateSettingsType, settings)
  }
}

const settingsMutations = {
  [updateSettingsType] (state, settings) {
    state.settings = settings
  }
}

export { settingsActions, settingsMutations }
