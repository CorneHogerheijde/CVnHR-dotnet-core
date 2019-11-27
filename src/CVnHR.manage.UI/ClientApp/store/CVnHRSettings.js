import axios from 'axios'

const updateSettingsType = 'updateSettingsType'
const ensureSettingsType = 'ensureSettingsType'
const uploadCertificateType = 'uploadCertificateType'

const settingsActions = {
  ensureSettings: async ({ commit }) => {
    commit(ensureSettingsType)
  },
  updateSettings: ({ commit }, settings) => {
    commit(updateSettingsType, settings)
  },
  uploadCertificate: ({ commit }, files) => {
    commit(uploadCertificateType, files)
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
  async [updateSettingsType] (state, settings) {
    try {
      await axios.put(`/api/settings/UpdateKvkApiSettings`, settings.kvkApiSettings)
      await axios.put(`/api/settings/UpdateHRDataServiceSettings`, settings.hrDataServiceSettings)
    } catch (err) {
      window.alert(err)
      console.log(err)
    }
    state.settings = settings
  },
  async [uploadCertificateType] (state, files) {
    // handle file changes
    const formData = new FormData()

    if (!files.length) return

    // append the files to FormData
    Array
      .from(Array(files.length).keys())
      .map(x => {
        formData.append('certificate', files[x], files[x].name)
      })

    // append the password
    formData.append('password', files.password)

    try {
      let response = await axios.post(`/api/settings/UpdateCertificate`, formData)
      state.settings = response.data
    } catch (err) {
      window.alert(err)
      console.log(err)
    }
  }
}

export { settingsActions, settingsMutations }
