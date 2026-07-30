/**
 * Gaza Real Estate Portal - Main JavaScript File
 */



// DOM CONTENT LOADED EVENT
document.addEventListener('DOMContentLoaded', function() {
  
  // 1. Mobile Menu Toggle
  const mobileMenuBtn = document.getElementById('mobileMenuBtn');
  const mobileMenu = document.getElementById('mobileMenu');
  if (mobileMenuBtn && mobileMenu) {
    mobileMenuBtn.addEventListener('click', function() {
      mobileMenu.style.display = mobileMenu.style.display === 'block' ? 'none' : 'block';
    });
  }

  // 2. Sidebar Toggle (Dashboards / Admin)
  const sidebarToggleBtn = document.getElementById('sidebarToggleBtn');
  if (sidebarToggleBtn) {
    sidebarToggleBtn.addEventListener('click', function() {
      const sidebar = document.querySelector('.sidebar');
      if (sidebar) {
        sidebar.classList.toggle('open');
      }
    });
  }

  // 3. Search Tabs (Home Page Hero)
  document.querySelectorAll('.tab-btn').forEach(btn => {
    btn.addEventListener('click', function() {
      document.querySelectorAll('.tab-btn').forEach(b => b.classList.remove('active'));
      this.classList.add('active');
    });
  });



  // 6. Details Page Contact Reveal (property-details.html)
  const callContactBtn = document.getElementById('callContactBtn');
  if (callContactBtn) {
    callContactBtn.addEventListener('click', function handler() {
      const phone = this.getAttribute('data-phone');
      const phoneText = document.getElementById('phoneText');
      if (phoneText) phoneText.textContent = phone;
      this.classList.add('btn-success');
      this.removeEventListener('click', handler);
      this.addEventListener('click', function() {
        window.location.href = `tel:${phone}`;
      });
    });
  }
  // 7. Add Property Image Validation & Upload Click (dashboard/add-property.html)
  const uploadZone = document.getElementById('uploadZone');
  const propertyImages = document.getElementById('propertyImages');
  if (uploadZone && propertyImages) {
    uploadZone.addEventListener('click', function() {
      propertyImages.click();
    });
  }

  if (propertyImages) {
    propertyImages.addEventListener('change', function() {
      const errorMsg = document.getElementById('imageError');
      const preview = document.getElementById('imagePreview');
      if (preview) preview.innerHTML = '';

      if (this.files.length < 3 || this.files.length > 5) {
        if (errorMsg) errorMsg.classList.remove('d-none');
        this.setCustomValidity('يجب اختيار من 3 إلى 5 صور للعقار.');
      } else {
        if (errorMsg) errorMsg.classList.add('d-none');
        this.setCustomValidity('');
        Array.from(this.files).forEach(file => {
          const reader = new FileReader();
          reader.onload = function(e) {
            if (preview) {
              preview.innerHTML += `<img src="${e.target.result}" class="rounded-3 border img-80-80">`;
            }
          };
          reader.readAsDataURL(file);
        });
      }
    });
  }

  const addPropertyForm = document.getElementById('addPropertyForm');
  if (addPropertyForm && propertyImages) {
    addPropertyForm.addEventListener('submit', function(e) {
      if (propertyImages.files.length < 3 || propertyImages.files.length > 5) {
        e.preventDefault();
        const errorMsg = document.getElementById('imageError');
        if (errorMsg) errorMsg.classList.remove('d-none');
        alert('يرجى التحقق من رفع من 3 إلى 5 صور للعقار للمتابعة.');
      }
    });
  }

  // 8. Edit Property Actions (dashboard/edit-property.html)
  // Delete current image
  document.querySelectorAll('.btn-delete-img').forEach(btn => {
    btn.addEventListener('click', function() {
      this.parentElement.remove();
    });
  });

  // 9. Dashboard Row Deletion Interaction (dashboard/user-index.html)
  const userPropertiesTable = document.getElementById('userPropertiesTable');
  if (userPropertiesTable) {
    userPropertiesTable.querySelectorAll('.action-btn-delete').forEach(btn => {
      btn.addEventListener('click', function() {
        const row = this.closest('tr');
        if (row && confirm('هل أنت متأكد من رغبتك في حذف هذا العقار؟')) {
          row.style.transition = 'all 0.4s ease';
          row.style.opacity = '0';
          row.style.transform = 'translateX(-20px)';
          setTimeout(() => {
            row.remove();
            // Decrement total stats
            const totalStat = document.querySelector('.stat-value');
            if (totalStat) {
              const val = parseInt(totalStat.textContent, 10);
              if (!isNaN(val) && val > 0) {
                totalStat.textContent = val - 1;
              }
            }
          }, 400);
        }
      });
    });
  }

  // 10. Admin Manage Properties Interaction (admin/manage-properties.html)
  const adminFilterStatus = document.getElementById('adminFilterStatus');
  const adminPropertiesTable = document.getElementById('adminPropertiesTable');
  if (adminFilterStatus && adminPropertiesTable) {
    adminFilterStatus.addEventListener('change', function() {
      const filterVal = this.value.trim();
      adminPropertiesTable.querySelectorAll('tbody tr').forEach(row => {
        const badgeSpan = row.querySelector('.badge-custom');
        if (badgeSpan) {
          const statusText = badgeSpan.textContent.trim();
          if (filterVal === 'الكل' || statusText === filterVal) {
            row.style.display = '';
          } else {
            row.style.display = 'none';
          }
        }
      });
    });
  }

  if (adminPropertiesTable) {
    // Approve Simulation
    adminPropertiesTable.querySelectorAll('.action-btn-approve').forEach(btn => {
      btn.addEventListener('click', function() {
        const row = this.closest('tr');
        const badge = row.querySelector('.badge-custom');
        if (badge) {
          badge.className = 'badge-custom badge-success';
          badge.textContent = 'معتمد';
        }
        this.style.transition = 'opacity 0.3s ease';
        this.style.opacity = '0';
        setTimeout(() => this.remove(), 300);
      });
    });

    // Reject/Delete Simulation
    adminPropertiesTable.querySelectorAll('.action-btn-delete').forEach(btn => {
      btn.addEventListener('click', function() {
        const row = this.closest('tr');
        const badge = row.querySelector('.badge-custom');
        if (badge) {
          badge.className = 'badge-custom badge-danger';
          badge.textContent = 'مرفوض';
        }
        this.style.transition = 'opacity 0.3s ease';
        this.style.opacity = '0';
        setTimeout(() => this.remove(), 300);
      });
    });
  }

  // 11. Property Gallery Thumbnails Click (property-details.html)
  const mainGalleryImg = document.getElementById('mainGalleryImg');
  if (mainGalleryImg) {
    document.querySelectorAll('.thumb-img').forEach(thumb => {
      thumb.addEventListener('click', function() {
        mainGalleryImg.src = this.src;
      });
    });
  }

  // 12. Home Page Search Transfer Form Submission Hook (index.html)
  // Removed: allow native form submission to Home/Properties

});
(function () {
  const form = document.getElementById('platformSettingsForm');
  const defaults = {
    platformName: 'بوابة غزة للعقارات',
    platformEmail: 'info@gazarealestate.ps',
    platformPhone: '+970 59 123 4567',
    platformAddress: 'غزة، فلسطين - شارع عمر المختار',
    primaryColor: '#1B3C74',
    maxImages: '5',
    propertiesPerPage: '9',
    featuredProperties: true,
    enableRegistration: true,
    enableEmailVerification: true,
    enableBrokerAccounts: false
  };
  // 13. Mobile Menu Toggle (index.html)
  const mobileMenuBtn = document.getElementById('mobileMenuBtn'); 
  const mobileMenu = document.getElementById('mobileMenu');
  if (mobileMenuBtn && mobileMenu) {
    mobileMenuBtn.addEventListener('click', function() {
      mobileMenu.classList.toggle('show');
    });
  }

  const logoZone = document.getElementById('logoUploadZone');
  const logoInput = document.getElementById('logoUploadInput');
  const logoPreview = document.getElementById('logoPreviewImg');
  const logoPlaceholder = document.getElementById('logoPlaceholder');
  const logoBox = document.getElementById('logoPreviewBox');
  const colorPicker = document.getElementById('primaryColor');
  const colorHex = document.getElementById('primaryColorHex');
  const colorBar = document.getElementById('colorPreviewBar');
  const saveToast = document.getElementById('saveToast');
  const toast = saveToast ? new bootstrap.Toast(saveToast, { delay: 3500 }) : null;

  function updateColorPreview(color) {
    if (colorHex) colorHex.value = color;
    if (colorBar) colorBar.style.background = 'linear-gradient(90deg, ' + color + ' 0%, var(--accent) 100%)';
  }

  if (colorPicker) {
    updateColorPreview(colorPicker.value);
    colorPicker.addEventListener('input', function () { updateColorPreview(this.value); });
  }

  if (logoZone && logoInput) {
    logoZone.addEventListener('click', function () { logoInput.click(); });
    logoZone.addEventListener('keydown', function (e) { if (e.key === 'Enter' || e.key === ' ') { e.preventDefault(); logoInput.click(); } });
    logoInput.addEventListener('change', function () {
      const file = this.files[0];
      if (!file) return;
      const reader = new FileReader();
      reader.onload = function (e) {
        logoPreview.src = e.target.result;
        logoPreview.classList.remove('d-none');
        logoPlaceholder.classList.add('d-none');
        logoBox.classList.add('has-logo');
      };
      reader.readAsDataURL(file);
    });
  }

  if (form) {
    form.addEventListener('submit', function (e) {
      e.preventDefault();
      const now = new Date();
      const saved = document.getElementById('lastSavedText');
      if (saved) {
        saved.textContent = now.toLocaleDateString('ar-EG', { day: 'numeric', month: 'long', year: 'numeric' }) +
          ' — ' + now.toLocaleTimeString('ar-EG', { hour: '2-digit', minute: '2-digit' });
      }
      if (toast) toast.show();
    });
  }

  document.getElementById('resetSettingsBtn')?.addEventListener('click', function () {
    if (!confirm('هل أنت متأكد من إعادة تعيين جميع الإعدادات إلى القيم الافتراضية؟')) return;
    document.getElementById('platformName').value = defaults.platformName;
    document.getElementById('platformEmail').value = defaults.platformEmail;
    document.getElementById('platformPhone').value = defaults.platformPhone;
    document.getElementById('platformAddress').value = defaults.platformAddress;
    document.getElementById('maxImages').value = defaults.maxImages;
    document.getElementById('propertiesPerPage').value = defaults.propertiesPerPage;
    document.getElementById('featuredProperties').checked = defaults.featuredProperties;
    document.getElementById('enableRegistration').checked = defaults.enableRegistration;
    document.getElementById('enableEmailVerification').checked = defaults.enableEmailVerification;
    document.getElementById('enableBrokerAccounts').checked = defaults.enableBrokerAccounts;
    if (colorPicker) { colorPicker.value = defaults.primaryColor; updateColorPreview(defaults.primaryColor); }
    logoPreview.classList.add('d-none');
    logoPreview.src = '';
    logoPlaceholder.classList.remove('d-none');
    logoBox.classList.remove('has-logo');
    logoInput.value = '';
  });

  document.getElementById('logoutAllDevicesBtn')?.addEventListener('click', function () {
    if (confirm('سيتم تسجيل خروجك من جميع الأجهزة ما عدا الجهاز الحالي. هل تريد المتابعة؟')) {
      document.querySelectorAll('.session-revoke-btn').forEach(function (btn) {
        btn.closest('.session-item')?.remove();
      });
      alert('تم تسجيل الخروج من جميع الأجهزة الأخرى بنجاح.');
    }
  });

  document.querySelectorAll('.session-revoke-btn').forEach(function (btn) {
    btn.addEventListener('click', function () {
      if (confirm('إنهاء هذه الجلسة؟')) this.closest('.session-item')?.remove();
    });
  });
})();

(function () {
  const tabBtns = document.querySelectorAll('.settings-tab-btn');
  const tabPanels = document.querySelectorAll('.tab-panel');

  tabBtns.forEach(function (btn) {
    btn.addEventListener('click', function () {
      const target = this.getAttribute('data-tab');
      tabBtns.forEach(function (b) { b.classList.remove('active'); });
      tabPanels.forEach(function (p) { p.classList.remove('active'); });
      this.classList.add('active');
      document.getElementById('tab-' + target)?.classList.add('active');
    });
  });

  function updateAvatarPreview(src) {
    ['profileAvatarImg', 'avatarTabPreview', 'sidebarAvatar'].forEach(function (id) {
      const el = document.getElementById(id);
      if (el) el.src = src;
    });
  }

  function handleAvatarFile(file) {
    if (!file || !file.type.startsWith('image/')) return;
    const reader = new FileReader();
    reader.onload = function (e) { updateAvatarPreview(e.target.result); };
    reader.readAsDataURL(file);
  }

  document.getElementById('quickAvatarUpload')?.addEventListener('change', function () {
    handleAvatarFile(this.files[0]);
  });

  const avatarZone = document.getElementById('avatarUploadZone');
  const avatarInput = document.getElementById('avatarUploadInput');
  if (avatarZone && avatarInput) {
    avatarZone.addEventListener('click', function () { avatarInput.click(); });
    avatarInput.addEventListener('change', function () { handleAvatarFile(this.files[0]); });
  }

  document.getElementById('saveAvatarBtn')?.addEventListener('click', function () {
    alert('تم حفظ الصورة الشخصية بنجاح!');
  });

  document.getElementById('personalInfoForm')?.addEventListener('submit', function (e) {
    e.preventDefault();
    const name = document.getElementById('fullName').value;
    const email = document.getElementById('email').value;
    const phone = document.getElementById('phone').value;
    const city = document.getElementById('city');
    const cityText = city.options[city.selectedIndex].text;
    const bio = document.getElementById('bio').value;
    document.getElementById('displayFullName').textContent = name;
    document.getElementById('displayEmail').textContent = email;
    document.getElementById('displayPhone').textContent = phone;
    document.getElementById('displayCity').textContent = cityText + ' — الرمال';
    document.getElementById('displayBio').textContent = bio;
    alert('تم حفظ المعلومات الشخصية بنجاح!');
  });

  const newPass = document.getElementById('newPasswordProfile');
  const strengthBar = document.getElementById('passwordStrengthBar');
  if (newPass && strengthBar) {
    newPass.addEventListener('input', function () {
      const len = this.value.length;
      let width = 0, color = '#dc3545';
      if (len >= 4) { width = 33; color = '#ffc107'; }
      if (len >= 8) { width = 66; color = '#17a2b8'; }
      if (len >= 12) { width = 100; color = '#28a745'; }
      strengthBar.style.width = width + '%';
      strengthBar.style.background = color;
    });
  }

  document.getElementById('passwordForm')?.addEventListener('submit', function (e) {
    e.preventDefault();
    const np = document.getElementById('newPasswordProfile').value;
    const cp = document.getElementById('confirmPasswordProfile').value;
    if (np !== cp) { alert('كلمتا المرور غير متطابقتين!'); return; }
    if (np.length < 8) { alert('يجب أن تكون كلمة المرور 8 أحرف على الأقل.'); return; }
    alert('تم تحديث كلمة المرور بنجاح!');
    this.reset();
    if (strengthBar) { strengthBar.style.width = '0'; }
  });
})();