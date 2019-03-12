import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

import { CoreModule } from '../core.module';
import { Registration } from '../models';
import { RestService } from './rest.service';

const FAKEREGISTRATIONS: Registration[] = [
  {
    id: '123',
    restrictedAccess: false,
    essFileNumber: 1289734,
    declarationAndConsent: false,
    dietaryNeeds: false,
    dietaryNeedsDetails: 'gluten intolerance',
    disasterAffectDetails: 'Freeform text',
    externalReferralsDetails: 'Freeform text',
    facility: '',
    familyRecoveryPlan: '',
    followUpDetails: '',
    insuranceCode: 'MANU120398',
    medicationNeeds: false,
    selfRegisteredDate: null,
    registrationCompletionDate: new Date('2019-03-11T20:48:31.246Z'),
    registeringFamilyMembers: 'yes',

    hasThreeDayMedicationSupply: true,
    hasInquiryReferral: false,
    hasHealthServicesReferral: false,
    hasFirstAidReferral: false,
    hasChildCareReferral: false,
    hasPersonalServicesReferral: false,
    hasPetCareReferral: false,
    hasPets: false,

    requiresAccommodation: false,
    requiresClothing: false,
    requiresFood: false,
    requiresIncidentals: false,
    requiresSupport: true,
    requiresTransportation: true,

    headOfHousehold:
    {
      id: 'qwertyuiop',
      firstName: 'John',
      lastName: 'Doe',
      nickname: 'Johnny',
      initials: 'JD',
      gender: 'male',
      dob: null,
      phoneNumber: '',
      phoneNumberAlt: '',
      personType: 'HOH',
      email: 'person@address.org',
      primaryResidence: null,
      mailingAddress: null
    },
    familyMembers: [
      {
        id: 'qwertyuiop',
        firstName: 'Jen',
        lastName: 'Borgnine',
        nickname: 'Iron Jen',
        initials: 'JB',
        gender: 'female',
        dob: null,
        personType: 'FMBR',
        relationshipToEvacuee: 'spouse',
        sameLastNameAsEvacuee: false
      }
    ],
    incidentTask: {
      id: 'aslkdfjh',
      taskNumber: '10293847',
      details: 'This is an incident task.',
      region: null,
      regionalDistrict: null,
      community: {
        id: 'zxoicuvz',
        name: 'Grand Forks',
        regionalDistrict: null
      }
    },
    hostCommunity: {
      id: 'aslkdfjs',
      name: 'Niagra',
      regionalDistrict: null
    },
    completedBy: null
  },
  {
    id: '123',
    restrictedAccess: true,
    essFileNumber: 1289734,
    declarationAndConsent: false,

    dietaryNeeds: false,
    dietaryNeedsDetails: 'gluten intolerance',
    disasterAffectDetails: 'Freeform text',
    externalReferralsDetails: 'Freeform text',
    facility: '',
    familyRecoveryPlan: '',
    followUpDetails: '',
    insuranceCode: 'MANU120398',
    medicationNeeds: false,
    selfRegisteredDate: null,
    registrationCompletionDate: null,
    registeringFamilyMembers: 'yes',

    hasThreeDayMedicationSupply: true,
    hasInquiryReferral: false,
    hasHealthServicesReferral: false,
    hasFirstAidReferral: false,
    hasChildCareReferral: false,
    hasPersonalServicesReferral: false,
    hasPetCareReferral: false,
    hasPets: false,

    requiresAccommodation: false,
    requiresClothing: false,
    requiresFood: false,
    requiresIncidentals: false,
    requiresSupport: true,
    requiresTransportation: true,

    headOfHousehold:
    {
      id: 'qwertyuiop',
      firstName: 'Barry',
      lastName: 'Placebo',
      nickname: 'Bipo',
      initials: 'BP',
      gender: 'Female',
      dob: null,
      phoneNumber: '',
      phoneNumberAlt: '',
      personType: 'HOH',
      email: 'person@address.org',
      primaryResidence: null,
      mailingAddress: null
    },
    familyMembers: [],
    incidentTask: {
      id: 'aslkdfjh',
      taskNumber: '10293847',
      details: 'This is an incident task.',
      region: null,
      regionalDistrict: null,
      community: {
        id: 'zxoicuvz',
        name: 'Hope',
        regionalDistrict: null
      }
    },
    hostCommunity: {
      id: 'aslkdfjs',
      name: 'Townland',
      regionalDistrict: null
    },
    completedBy: null
  }
];

@Injectable({
  providedIn: CoreModule
})
export class RegistrationService extends RestService {

  getRegistries(page?: number, recordLimit?: number): Observable<Registration[]> {
    // records and page are set limits on the query number
    if (!recordLimit) { recordLimit = 100; }
    if (!page) { page = 1; }

    return of(FAKEREGISTRATIONS);
  }
}